param(
    $SelfHosted = 0
)

$global:ProgressPreference = 'SilentlyContinue'

Write-Host "Updating Nodegem services to latest..."

$nodegem_web_static_files = "https://gitlab.com/nodegem/nodegem-web/-/jobs/artifacts/master/download?job=build_and_deploy_static"
$download_host = "https://cdn.nodegem.io/releases/latest"
$client_service_download = "$download_host/client_service"
$web_api_download = "$download_host/web_api"
$service_name = "Nodegem_ClientService"
$service_name_api = "Nodegem_WebApi"

$download_file = "$client_service_download/nodegem-win64.zip"
Write-Host "Downloading Nodegem Client Service ($download_file)..."
Invoke-WebRequest $download_file -OutFile 'nodegem-win64.zip'

if ($SelfHosted) {
    Write-Host "Downloading Nodegem Web Client ($nodegem_web_static_files)..."
    Invoke-WebRequest $nodegem_web_static_files -OutFile 'nodegem-web-client.zip'

    $web_api_download_file = "$web_api_download/nodegem-webapi-win64.zip"
    Write-Host "Downloading Nodegem Web Api Service ($web_api_download_file)..."
    Invoke-WebRequest $web_api_download_file -OutFile 'nodegem-webapi-win64.zip'

    Stop-Service $service_name_api -Force
}

if (Get-Service $service_name -ErrorAction SilentlyContinue) {
    Stop-Service $service_name -Force
}

Write-Host "Unzipping file..."
Expand-Archive -LiteralPath "nodegem-win64.zip" -DestinationPath "$env:APPDATA/Nodegem" -Force

if ($SelfHosted) {
    Remove-Item -Recurse "$env:APPDATA/Nodegem/WebApi"
    Expand-Archive -LiteralPath "nodegem-webapi-win64.zip" -DestinationPath "$env:APPDATA/Nodegem/WebApi" -Force
    Expand-Archive -LiteralPath "nodegem-web-client.zip" -DestinationPath "$env:APPDATA/Nodegem/WebApi/wwwroot" -Force
    Get-ChildItem "$env:APPDATA/Nodegem/WebApi/wwwroot/build" -Recurse | Move-Item -Destination "$env:APPDATA/Nodegem/WebApi/wwwroot"
    Remove-Item -Recurse "$env:APPDATA/Nodegem/WebApi/wwwroot/build"

    Restart-Service -Name $service_name_api
}

Restart-Service -Name $service_name

if ($SelfHosted) {
    if ((Get-Service -Name $service_name_api).Status -ne "Running") {
        Write-Host "Successfully updated Nodegem Web Api to latest!"
    }
    else {
        Write-Host "Nodegem Web Api was unable to restart"    
    }
}

if ((Get-Service -Name $service_name).Status -eq "Running") {
    Write-Host "Successfully updated Nodegem to latest!"
}
else {
    Write-Host "Nodegem was unable to restart"
}

Write-Host "Some cleanup..."
Remove-Item "nodegem-win64.zip" -Force

if ($SelfHosted) {
    Remove-Item "nodegem-webapi-win64.zip" -Force
    Remove-Item "nodegem-web-client.zip" -Force
}