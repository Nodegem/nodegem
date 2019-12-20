param(
    [Boolean]$selfHosted = 0
)

Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
$ProgressPreference = 'SilentlyContinue'

$nodegem_web_static_files = "https://gitlab.com/nodegem/nodegem-web/-/jobs/artifacts/master/download?job=build_and_deploy_static"
$download_host = "https://cdn.nodegem.io/releases/latest"
$client_service_download = "$download_host/client_service"
$web_api_download = "$download_host/web_api"
$service_name = "Nodegem"
$service_name_api = "Nodegem API"

Write-Host "Downloading .NET Core 3.0..."
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'
Write-Host "Installing .NET Core 3.0..."
& "./dotnet-install.ps1" -Channel 'Current' -Runtime dotnet

$download_file = "$client_service_download/nodegem-win64.zip"
Write-Host "Downloading Nodegem Client Service ($download_file)..."
Invoke-WebRequest $download_file -OutFile 'nodegem-win64.zip'

if (Get-Service $service_name -ErrorAction SilentlyContinue) {
    Stop-Service $service_name -Force
    sc.exe delete $service_name
}

if ($selfHosted) {
    Write-Host "Downloading Nodegem Web Client ($nodegem_web_static_files)..."
    Invoke-WebRequest $nodegem_web_static_files -OutFile 'nodegem-web-client.zip'

    $web_api_download_file = "$web_api_download/nodegem-webapi-win64.zip"
    Write-Host "Downloading Nodegem Web Api Service ($web_api_download_file)..."
    Invoke-WebRequest $web_api_download_file -OutFile 'nodegem-webapi-win64.zip'
}

Write-Host "Unzipping file(s)..."
Expand-Archive -LiteralPath "nodegem-win64.zip" -DestinationPath "$env:APPDATA/Nodegem/ClientService" -Force

if ($selfHosted) {
    Expand-Archive -LiteralPath "nodegem-webapi-win64.zip" -DestinationPath "$env:APPDATA/Nodegem/WebApi" -Force
    Expand-Archive -LiteralPath "nodegem-web-client.zip" -DestinationPath "$env:APPDATA/Nodegem/WebApi/wwwroot" -Force
}


$username = Read-Host "Nodegem username: "
$password = Read-Host -assecurestring "Please enter your password"
$password_confirm = Read-Host -assecurestring "Please enter your password"

while (-Not $password -eq $password_confirm) {
    Write-Host "Passwords aren't the same"
    $password = Read-Host -assecurestring "Please enter your password"
    $password_confirm = Read-Host -assecurestring "Please enter your password"
}

$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
$pass = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)

$additionalFlag = ""
if ($selfHosted) {
    $api_service_description = "The service that stores and retrieves user and graph information"
    $api_params = @{
        Name           = $service_name_api
        BinaryPathName = "$env:APPDATA/Nodegem/WebApi/WebApi"
        DisplayName    = "Nodegem API"
        Description    = $api_service_description
    }
    $additionalFlag = "-e http://localhost:5000"
}

$service_description = "The service that bridges the web client and the api service"
$params = @{
    Name           = $service_name
    BinaryPathName = "$env:APPDATA/Nodegem/ClientService/Nodegem.ClientService.exe -u $username -p $pass $additionalFlag"
    DisplayName    = "Nodegem Client" 
    Description    = $service_description 
}

$logFileExists = Get-EventLog -list | Where-Object { $_.logdisplayname -eq $service_name } 
if (! $logFileExists) {
    Write-Host "Adding event log: $service_name"
    New-EventLog -LogName $service_name -Source NodegemSource
}

New-Service @params
Set-Service -Name $service_name -StartupType Automatic
Restart-Service -Name $service_name

Write-Host "Successfully installed Nodegem!!!"

Write-Host "Some cleanup..."
Remove-Item "nodegem-win64.zip" -Force
Remove-Item "dotnet-install.ps1" -Force
Remove-Item "nodegem-webapi-win64.zip" -Force
Remove-Item "nodegem-web-client.zip" -Force