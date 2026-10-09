// ============================================================
// Contoso Support — Main Bicep Deployment
// Deploys all Azure infrastructure for the platform
// ============================================================

param location string = resourceGroup().location
param projectName string = 'contosov5'
param environment string = 'dev'

// Tags applied to all resources
var tags = {
  project: 'contoso-support'
  environment: environment
  managedBy: 'bicep'
}

// ============================================================
// 1. Azure AI Foundry (AI Platform)
// ============================================================
resource aiFoundry 'Microsoft.CognitiveServices/accounts@2026-09-01' = {
  name: '${projectName}-ai-foundry'
  location: location
  kind: 'AIServices'
  tags: tags
  sku: {
    name: 'S0'
  }
  identity: {
    type: 'SystemAssigned'
  }
  properties: {
    customSubDomainName: '${projectName}-foundry'
    allowProjectManagement: true
    publicNetworkAccess: 'Enabled'
  }
}

// AI Project (sub-resource of Foundry)
resource aiProject 'Microsoft.CognitiveServices/accounts/projects@2026-09-01' = {
  parent: aiFoundry
  name: '${projectName}-project'
  location: location
  identity: {
    type: 'SystemAssigned'
  }
  properties: {}
}

// Model deployment
resource gptModel 'Microsoft.CognitiveServices/accounts/deployments@2026-09-01' = {
  parent: aiFoundry
  name: 'gpt-5.4-nano'
  dependsOn: [
    aiProject
  ]
  sku: {
    name: 'GlobalStandard'
    capacity: 8
  }
  properties: {
    model: {
      format: 'OpenAI'
      name: 'gpt-5.4-nano'
      version: '2026-03-17'
    }
  }
}

// ============================================================
// 2. Log Analytics Workspace (shared by App Insights + Container Apps)
// ============================================================
resource logAnalytics 'Microsoft.OperationalInsights/workspaces@2023-09-01' = {
  name: '${projectName}-logs'
  location: location
  tags: tags
  properties: {
    sku: {
      name: 'PerGB2018'
    }
    retentionInDays: 30
  }
}

// ============================================================
// 3. Application Insights (Telemetry)
// ============================================================
resource appInsights 'Microsoft.Insights/components@2020-02-02' = {
  name: '${projectName}-insights'
  location: location
  tags: tags
  kind: 'web'
  properties: {
    Application_Type: 'web'
    WorkspaceResourceId: logAnalytics.id
  }
}

// ============================================================
// 4. Azure Key Vault (Secrets)
// ============================================================
resource keyVault 'Microsoft.KeyVault/vaults@2023-07-01' = {
  name: '${projectName}-kv'
  location: location
  tags: tags
  properties: {
    tenantId: subscription().tenantId
    sku: {
      family: 'A'
      name: 'standard'
    }
    enableRbacAuthorization: true
    enableSoftDelete: true
    softDeleteRetentionInDays: 7
    publicNetworkAccess: 'Enabled'
  }
}

// ============================================================
// 5. Azure Container Registry (Docker images)
// ============================================================
resource acr 'Microsoft.ContainerRegistry/registries@2023-07-01' = {
  name: '${projectName}acr'
  location: location
  tags: tags
  sku: {
    name: 'Basic'
  }
  properties: {
    adminUserEnabled: true
    publicNetworkAccess: 'Enabled'
  }
}

// ============================================================
// 6. Container Apps Environment
// ============================================================
resource containerAppsEnv 'Microsoft.App/managedEnvironments@2024-03-01' = {
  name: '${projectName}-env'
  location: location
  tags: tags
  properties: {
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
output foundryEndpoint string = aiFoundry.properties.endpoint
output acrLoginServer string = acr.properties.loginServer
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output keyVaultUri string = keyVault.properties.vaultUri
