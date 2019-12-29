#!/bin/bash

if ! [ -x "$(command -v dotnet)" ] || ! [ "$(dotnet --info | grep 3.0)" ]; then
  echo 'Installing .NET Core 3.0...' >&2
  wget 'https://dotnetwebsite.azurewebsites.net/download/dotnet-core/scripts/v1/dotnet-install.sh'
  chmod +x dotnet-install.sh
  source dotnet-install.sh --install-dir /usr/share/dotnet

  ln -sfn /usr/share/dotnet/dotnet /usr/bin/dotnet
  
  if ! [ -x "$(command -v dotnet)" ]; then
	  echo 'Was unsuccessful in installing .NET Core 3.0'
	  exit 1
  fi

  echo 'Successfully installed .NET Core!'
else
  echo '.NET Core >= 3.0 already installed'
fi

SERVICE_USER=nodegemserviceuser
NODEGEM_HOST='https://cdn.nodegem.io'

echo 'Downloading Nodegem API...'
unameArch=$(uname -m)
case "$unameArch" in
    amd64)     NODEGEM_ARCH="64";;
    x86_64)    NODEGEM_ARCH="64";;
    aarch64)   NODEGEM_ARCH="64";;
    armv7l)    NODEGEM_ARCH="32";;
    * )        echo "Your Architecture '$unameArch' -> IS NOT SUPPORTED.";;
esac

if [[ "$unameArch" == "aarch64" || "$unameArch" == "armv7l" ]]; then
    NODEGEM_API_FILE=nodegem-webapi-linux-arm$NODEGEM_ARCH.tar.gz
else
    NODEGEM_API_FILE=nodegem-webapi-linux$NODEGEM_ARCH.tar.gz
fi

NODEGEM_WEBAPI_DOWNLOAD_URL=$NODEGEM_HOST/releases/latest/web_api/$NODEGEM_API_FILE

echo 'Downloading ' $NODEGEM_WEBAPI_DOWNLOAD_URL
curl $NODEGEM_WEBAPI_DOWNLOAD_URL -o $NODEGEM_API_FILE

if ! id -u $SERVICE_USER > /dev/null 2>&1; then
	echo 'Adding a service user...' 
	useradd -m -d /var/nodegem $SERVICE_USER
  mkdir -p /var/nodegem/webapi

  if [ ! -e "/var/nodegem/vars.env" ]; then 
    touch /var/nodegem/vars.env
  fi
fi

tar -zxf $NODEGEM_API_FILE -C /var/nodegem/webapi
chown -R $SERVICE_USER:$SERVICE_USER /var/nodegem

# This allows the single file executable to start without running into permissions issues
mkdir -p /var/tmp/.net
chmod -R 777 /var/tmp/.net/

if systemctl list-units --full -all | grep -Fq 'nodegem_webapi.service'; then
  echo "Shutting down 'nodegem_webapi'"
  systemctl unmask nodegem_webapi
  systemctl stop nodegem_webapi
  systemctl disable nodegem_webapi
fi

echo 'Downloading ' $NODEGEM_HOST/install/linux/nodegem_webapi.service
curl -Lq $NODEGEM_HOST/install/linux/nodegem_webapi.service -o nodegem_webapi.service

echo 'Moving downloaded service file to services folder...'
cp -f nodegem_webapi.service /etc/systemd/system/nodegem_webapi.service

echo 'Spinning up new daemon...'
systemctl daemon-reload

systemctl enable nodegem_webapi
systemctl restart nodegem_webapi