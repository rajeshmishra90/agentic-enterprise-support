// ============================================================
// Contoso Support — Core Infrastructure Bicep Deployment
// Deploys AI Foundry, ACR, Key Vault, and Monitoring
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
// Outputs
// ============================================================
output foundryEndpoint string = aiFoundry.properties.endpoint
output acrLoginServer string = acr.properties.loginServer
output appInsightsConnectionString string = appInsights.properties.ConnectionString
output keyVaultUri string = keyVault.properties.vaultUri
