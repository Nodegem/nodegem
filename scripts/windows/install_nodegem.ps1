param(
    $SelfHosted = 0
)

$global:ProgressPreference = 'SilentlyContinue'

$nodegem_web_static_files = "https://gitlab.com/nodegem/nodegem-web/-/jobs/artifacts/master/download?job=build_and_deploy_static"
$download_host = "https://cdn.nodegem.io/releases/latest"
$client_service_download = "$download_host/client_service"
$web_api_download = "$download_host/web_api"
$service_name = "Nodegem_ClientService"
$service_name_api = "Nodegem_WebApi"

Write-Host "Downloading .NET Core 3.x..."
Invoke-WebRequest 'https://dot.net/v1/dotnet-install.ps1' -OutFile 'dotnet-install.ps1'
Write-Host "Installing .NET Core 3.x..."
& "./dotnet-install.ps1" -Channel 'Current' -Runtime dotnet

$download_file = "$client_service_download/nodegem-win64.zip"
Write-Host "Downloading Nodegem Client Service ($download_file)..."
Invoke-WebRequest $download_file -OutFile 'nodegem-win64.zip'

if (Get-Service $service_name_api -ErrorAction SilentlyContinue) {
    Stop-Service $service_name_api -Force
    sc.exe delete $service_name_api
}

if (Get-Service $service_name -ErrorAction SilentlyContinue) {
    Stop-Service $service_name -Force
    sc.exe delete $service_name
}

if ($SelfHosted) {
    Write-Host "Downloading Nodegem Web Client ($nodegem_web_static_files)..."
    Invoke-WebRequest $nodegem_web_static_files -OutFile 'nodegem-web-client.zip'

    $web_api_download_file = "$web_api_download/nodegem-webapi-win64.zip"
    Write-Host "Downloading Nodegem Web Api Service ($web_api_download_file)..."
    Invoke-WebRequest $web_api_download_file -OutFile 'nodegem-webapi-win64.zip'
}

Write-Host "Unzipping file(s)..."
Expand-Archive -LiteralPath "nodegem-win64.zip" -DestinationPath "$env:APPDATA/Nodegem/ClientService" -Force

if ($SelfHosted) {
    Remove-Item -Recurse "$env:APPDATA/Nodegem/WebApi"
    Expand-Archive -LiteralPath "nodegem-webapi-win64.zip" -DestinationPath "$env:APPDATA/Nodegem/WebApi" -Force
    Expand-Archive -LiteralPath "nodegem-web-client.zip" -DestinationPath "$env:APPDATA/Nodegem/WebApi/wwwroot" -Force
    Get-ChildItem "$env:APPDATA/Nodegem/WebApi/wwwroot/build" -Recurse | Move-Item -Destination "$env:APPDATA/Nodegem/WebApi/wwwroot"
    Remove-Item -Recurse "$env:APPDATA/Nodegem/WebApi/wwwroot/build"
}

if (-Not $SelfHosted) {
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
}
else {
    $username = "Nodegem_Default"
    $pass = "P@ssword1"
}

$logFileExists = Get-EventLog -list | Where-Object { $_.logdisplayname -eq "Nodegem" } 
if (! $logFileExists) {
    Write-Host "Adding event log: Nodegem"
    New-EventLog -LogName "Nodegem" -Source @("Nodegem.WebApi", "Nodegem.ClientService")
}

$additionalFlag = ""
if ($SelfHosted) {
    New-Service -Name $service_name_api -BinaryPathName "$env:APPDATA/Nodegem/WebApi/Nodegem.WebApi.exe --selfHosted=true" `
        -Description "The API service that handles the website and api requests" `
        -DisplayName "Nodegem API" -StartupType Automatic
    Restart-Service -Name $service_name_api

    $additionalFlag = "-e http://localhost:5000"
}

New-Service -Name $service_name -BinaryPathName "$env:APPDATA/Nodegem/ClientService/Nodegem.ClientService.exe -u $username -p $pass $additionalFlag" `
    -Description "The service that bridges the web client to the api service and runs the graphs" `
    -DisplayName "Nodegem Client" -StartupType Automatic
Restart-Service -Name $service_name

Write-Host "Successfully installed Nodegem!!!"

Write-Host "Some cleanup..."
Remove-Item "nodegem-win64.zip" -Force
Remove-Item "dotnet-install.ps1" -Force

if ($SelfHosted) {
    Remove-Item "nodegem-webapi-win64.zip" -Force
    Remove-Item "nodegem-web-client.zip" -Force
}
