# infra/scripts/teardown.ps1
# This script deletes all resources inside the resource group without deleting the group itself.

$resourceGroup = "rg-contoso-support-kc"

Write-Host "Fetching all resources in $resourceGroup..."
$resources = az resource list --resource-group $resourceGroup --query "[].id" -o tsv

if (-not $resources) {
    Write-Host "No resources found in $resourceGroup."
    exit
}

foreach ($resource in $resources) {
    Write-Host "Deleting $resource..."
    az resource delete --ids $resource
}

Write-Host "Teardown complete!"
