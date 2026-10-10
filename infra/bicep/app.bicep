// ============================================================
// Contoso Support — App Infrastructure Bicep Deployment
// Deploys Container Apps and RBAC roles
// ============================================================

param location string = resourceGroup().location
param projectName string = 'contosov5'
param environment string = 'dev'

var tags = {
  project: 'contoso-support'
  environment: environment
  managedBy: 'bicep'
}

// ============================================================
// Existing Resources
// ============================================================
resource aiFoundry 'Microsoft.CognitiveServices/accounts@2026-09-01' existing = {
  name: '${projectName}-ai-foundry'
}
resource aiProject 'Microsoft.CognitiveServices/accounts/projects@2026-09-01' existing = {
  name: '${projectName}-project'
}
resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' existing = {
  name: '${projectName}-logs'
}
resource appInsights 'Microsoft.Insights/components@2020-02-02' existing = {
  name: '${projectName}-insights'
}
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' existing = {
  name: '${projectName}acr'
}

// ============================================================
// 6. Container Apps Environment
// ============================================================
resource containerAppsEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: '${projectName}-env'
  location: location
  tags: tags
  properties: {
    workloadProfiles: [
      {
        name: 'Consumption'
        workloadProfileType: 'Consumption'
      }
    ]
    appLogsConfiguration: {
      destination: 'log-analytics'
      logAnalyticsConfiguration: {
        customerId: logAnalytics.properties.customerId
        sharedKey: logAnalytics.listKeys().primarySharedKey
      }
    }
  }
}

// ============================================================
// 7. Container App — API
// ============================================================
resource apiApp 'Microsoft.App/containerApps@2024-03-01' = {
  name: '${projectName}-api'
  location: location
  tags: tags
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    managedEnvironmentId: containerAppsEnv.id
    configuration: {
      ingress: {
        external: true
        targetPort: 8080
        transport: 'auto'
      }
      registries: [
        {
          server: acr.properties.loginServer
          username: acr.listCredentials().username
          passwordSecretRef: 'acr-password'
        }
      ]
      secrets: [
        {
          name: 'acr-password'
          value: acr.listCredentials().passwords[0].value
        }
      ]
    }
    template: {
      containers: [
        {
          name: 'contoso-api'
          image: '${acr.properties.loginServer}/contoso-api:latest'
          resources: {
            cpu: json('0.5')
            memory: '1Gi'
          }
          env: [
            {
              name: 'ASPNETCORE_ENVIRONMENT'
              value: environment == 'prod' ? 'Production' : 'Development'
            }
            {
              name: 'AzureAI__Endpoint'
              value: '${aiFoundry.properties.endpoint}api/projects/${aiProject.name}'
            }
            {
              name: 'APPLICATIONINSIGHTS_CONNECTION_STRING'
              value: appInsights.properties.ConnectionString
            }
          ]
        }
      ]
      scale: {
        minReplicas: 0
        maxReplicas: 3
        rules: [
          {
            name: 'http-scaling'
            http: {
              metadata: {
                concurrentRequests: '50'
              }
            }
          }
        ]
      }
    }
  }
}

// ============================================================
// 8. Role Assignments (RBAC)
// ============================================================
// Cognitive Services OpenAI User
var cognitiveServicesOpenAiUserRoleId = '5e0bd9bd-7b93-4f28-af87-19fc36ad61bd'

resource roleAssignment 'Microsoft.Authorization/roleAssignments@2022-04-01' = {
  name: guid(aiFoundry.id, apiApp.id, cognitiveServicesOpenAiUserRoleId)
  scope: aiFoundry
  properties: {
    roleDefinitionId: subscriptionResourceId('Microsoft.Authorization/roleDefinitions', cognitiveServicesOpenAiUserRoleId)
    principalId: apiApp.identity.principalId
    principalType: 'ServicePrincipal'
  }
}

// ============================================================
// Outputs
// ============================================================
output apiUrl string = 'https://${apiApp.properties.configuration.ingress.fqdn}'
