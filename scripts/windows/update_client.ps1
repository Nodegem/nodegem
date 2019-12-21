Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
$ProgressPreference = 'SilentlyContinue'

$download_host = "https://cdn.nodegem.io/releases/latest/client_service"
$service_name = "Nodegem"

$download_file = "$download_host/nodegem-win64.zip"
Write-Host "Downloading Nodegem Client Service ($download_file)..."
Invoke-WebRequest "$download_host/nodegem-win64.zip" -OutFile 'nodegem-win64.zip'

if (Get-Service $service_name -ErrorAction SilentlyContinue) {
    Stop-Service $service_name -Force
}

Write-Host "Unzipping file..."
Expand-Archive -LiteralPath "nodegem-win64.zip" -DestinationPath "$env:APPDATA/Nodegem" -Force

Restart-Service -Name $service_name

Write-Host "Successfully updated Nodegem to latest!"

Write-Host "Some cleanup..."
Remove-Item "nodegem-win64.zip" -Force