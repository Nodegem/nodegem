Set-ExecutionPolicy -ExecutionPolicy Bypass -Scope Process
$ProgressPreference = 'SilentlyContinue'

$download_host = "https://cdn.nodegem.io/releases/latest"
$client_service_download = "$download_host/client_service"
$service_name = "Nodegem"

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

Write-Host "Unzipping file..."
Expand-Archive -LiteralPath "nodegem-win64.zip" -DestinationPath "$env:APPDATA/Nodegem" -Force


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

$service_description = "The service that bridges the web client and the api service"
$params = @{
    Name           = $service_name
    BinaryPathName = "$env:APPDATA/Nodegem/Nodegem.ClientService.exe -u $username -p $pass"
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