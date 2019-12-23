param(
    $SelfHosted = 0
)

Write-Host "Changing Nodegem service account..."

$username = Read-Host "Nodegem username: "
$password = Read-Host -assecurestring "Please enter your password"
$password_confirm = Read-Host -assecurestring "Please enter your password"

while (-Not $password -eq $password_confirm) {
    Write-Host "Passwords aren't the same"
    $password = Read-Host -assecurestring "Please enter your password"
    $password_confirm = Read-Host -assecurestring "Please enter your password"
}

$BSTR = [System.Runtime.InteropServices.Marshal]::SecureStringToBSTR($password)
$password = [System.Runtime.InteropServices.Marshal]::PtrToStringAuto($BSTR)

$additionalParams = ""
if ($SelfHosted) {
    $additionalParams = "-e http://localhost:5000"
}

Stop-Service -Name "Nodegem_ClientService"
sc.exe config "Nodegem_ClientService" binPath="$env:APPDATA/Nodegem/ClientService/Nodegem.ClientService.exe -u $username -p $password $additionalParams"
Restart-Service -Name "Nodegem_ClientService"