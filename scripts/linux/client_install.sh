#!/bin/bash

NODEGEM_HOST='https://cdn.nodegem.io'
NODEGEM_DOWNLOAD_PATH=releases/latest/client_service
NODEGEM_CLIENT_FOLDER_NAME=nodegem_client
SERVICE_USER=nodegemserviceuser

if ! [ -x "$(command -v dotnet)" ] || ! [ "$(dotnet --info | grep 3.0)" ]; then
  echo 'Installing .NET Core 3.0...' >&2
  wget 'https://dotnetwebsite.azurewebsites.net/download/dotnet-core/scripts/v1/dotnet-install.sh'
  chmod +x dotnet-install.sh
  source dotnet-install.sh --channel Current --install-dir /usr/share/dotnet --runtime dotnet

  ln -sfn /usr/share/dotnet/dotnet /usr/bin/dotnet
  
  if ! [ -x "$(command -v dotnet)" ]; then
	  echo 'Was unsuccessful in installing .NET Core 3.0'
	  exit 1
  fi

  echo 'Successfully installed .NET Core!'
else
  echo '.NET Core >= 3.0 already installed'
fi

echo 'Downloading Nodegem service client...'
unameOut=$(uname -s)
case "$unameOut" in
    Linux*)      NODEGEM_KERNEL="linux";;
    Darwin*)     NODEGEM_KERNEL="osx";;
    * )          echo "Your Operating System -> ITS NOT SUPPORTED";;
esac

unameArch=$(uname -m)
case "$unameArch" in
    amd64)     NODEGEM_ARCH="64";;
    x86_64)    NODEGEM_ARCH="64";;
    aarch64)   NODEGEM_ARCH="64";;
    armv7l)    NODEGEM_ARCH="32";;
    * )        echo "Your Architecture '$unameArch' -> ITS NOT SUPPORTED.";;
esac

if [[ "$unameArch" == "aarch64" || "$unameArch" == "armv7l" ]]; then
    NODEGEM_FILE=nodegem-$NODEGEM_KERNEL-arm$NODEGEM_ARCH.tar.gz
else
    NODEGEM_FILE=nodegem-$NODEGEM_KERNEL$NODEGEM_ARCH.tar.gz
fi

NODEGEM_DOWNLOAD_URL=$NODEGEM_HOST/$NODEGEM_DOWNLOAD_PATH/$NODEGEM_FILE

echo 'Downloading ' $NODEGEM_DOWNLOAD_URL
curl $NODEGEM_DOWNLOAD_URL -o $NODEGEM_FILE

if ! id -u $SERVICE_USER > /dev/null 2>&1; then
	echo 'Adding a service user...' 
	useradd -m -d /var/nodegem $SERVICE_USER
	mkdir -p /var/nodegem
fi

tar -zxf $NODEGEM_FILE -C /var/nodegem/
chown -R $SERVICE_USER:$SERVICE_USER /var/nodegem

# This allows the single file executable to start without running into permissions issues
mkdir -p /var/tmp/.net
chmod -R 777 /var/tmp/.net/
read -p "Username: " nodegem_user
while true; do
    read -s -p "Password: " nodegem_password
    echo
    read -s -p "Password (again): " nodegem_password2
    echo
    [ "$nodegem_password" = "$nodegem_password2" ] && break
    echo "Please try again"
done

echo 'Nodegem credentials...'
curl $NODEGEM_HOST/install/linux/nodegem.service.template -o nodegem.service.template
sed -e "s/\${username}/${nodegem_user}/" -e "s/\${password}/${nodegem_password}/" nodegem.service.template >> nodegem.service
rm nodegem.service.template

echo 'Setting up daemon...'

cp -f nodegem.service /etc/systemd/system
rm nodegem.service
rm $NODEGEM_FILE
systemctl daemon-reload
systemctl enable nodegem
systemctl restart nodegem

echo 'Installation complete!'

