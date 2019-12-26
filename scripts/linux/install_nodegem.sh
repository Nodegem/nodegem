#!/bin/bash

self_hosted=false
while [ $# -gt 0 ]; do
  case "$1" in
    --self-hosted=*)
      self_hosted="${1#*=}"
      ;;
    *)
      printf "Invalid argument"
      exit 1
  esac
  shift
done

NODEGEM_WEB_CLIENT_FILE="https://gitlab.com/nodegem/nodegem-web/-/jobs/artifacts/master/download?job=build_and_deploy_static"
NODEGEM_HOST='https://cdn.nodegem.io'
NODEGEM_DOWNLOAD_PATH=releases/latest/client_service
NODEGEM_CLIENT_FOLDER_NAME=nodegem_client
SERVICE_USER=nodegemserviceuser

if [ $self_hosted = true ]; then
  apt install -y unzip
fi

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
unameArch=$(uname -m)
case "$unameArch" in
    amd64)     NODEGEM_ARCH="64";;
    x86_64)    NODEGEM_ARCH="64";;
    aarch64)   NODEGEM_ARCH="64";;
    armv7l)    NODEGEM_ARCH="32";;
    * )        echo "Your Architecture '$unameArch' -> IS NOT SUPPORTED.";;
esac

if [[ "$unameArch" == "aarch64" || "$unameArch" == "armv7l" ]]; then
    NODEGEM_FILE=nodegem-linux-arm$NODEGEM_ARCH.tar.gz
    NODEGEM_API_FILE=nodegem-webapi-linux-arm$NODEGEM_ARCH.tar.gz
else
    NODEGEM_FILE=nodegem-linux$NODEGEM_ARCH.tar.gz
    NODEGEM_API_FILE=nodegem-webapi-linux$NODEGEM_ARCH.tar.gz
fi

NODEGEM_DOWNLOAD_URL=$NODEGEM_HOST/$NODEGEM_DOWNLOAD_PATH/$NODEGEM_FILE
NODEGEM_WEBAPI_DOWNLOAD_URL=$NODEGEM_HOST/releases/latest/web_api/$NODEGEM_API_FILE

if [ $self_hosted = true ]; then
  curl -L $NODEGEM_WEBAPI_DOWNLOAD_URL -o $NODEGEM_API_FILE
  curl -L $NODEGEM_WEB_CLIENT_FILE -o web_client.zip
fi

echo 'Downloading ' $NODEGEM_DOWNLOAD_URL
curl $NODEGEM_DOWNLOAD_URL -o $NODEGEM_FILE

if ! id -u $SERVICE_USER > /dev/null 2>&1; then
	echo 'Adding a service user...' 
	useradd -m -d /var/nodegem $SERVICE_USER
	mkdir -p /var/nodegem/webapi
  mkdir -p /var/nodegem/webapi/wwwroot
  mkdir -p /var/nodegem/client
  if [ ! -e "/var/nodegem/vars.env" ]; then 
    touch /var/nodegem/vars.env
  fi
fi

systemctl unmask nodegem_webapi
systemctl stop nodegem_webapi
systemctl disable nodegem_webapi

systemctl unmask nodegem
systemctl stop nodegem
systemctl disable nodegem

if [ $self_hosted = true ]; then
  tar -zxf $NODEGEM_API_FILE -C /var/nodegem/webapi
  unzip web_client.zip -d temp_web_client/
  cp -rp temp_web_client/build/* /var/nodegem/webapi/wwwroot/
  rm -rf temp_web_client
  rm web_client.zip
fi

tar -zxf $NODEGEM_FILE -C /var/nodegem/client
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

if [ $self_hosted = true ]; then
    extraArgs="-e http:\/\/localhost:5000"
    sed -e "s/\${username}/${nodegem_user}/" -e "s/\${password}/${nodegem_password}/" -e "s/\${extra}/${extraArgs}/" nodegem.service.template > nodegem.service
else
    extraArgs=""
    sed -e "s/\${username}/${nodegem_user}/" -e "s/\${password}/${nodegem_password}/" -e "s/\${extra}/${extraArgs}/" nodegem.service.template > nodegem.service
fi

rm nodegem.service.template

echo 'Setting up daemon...'

if [ $self_hosted = true ]; then
  curl -q $NODEGEM_HOST/install/linux/nodegem_webapi.service -o /etc/systemd/system/nodegem_webapi.service
  cp -f nodegem.service /etc/systemd/system
  rm -f nodegem.service nodegem_webapi.service $NODEGEM_FILE $NODEGEM_API_FILE
fi

systemctl daemon-reload

systemctl enable nodegem
systemctl restart nodegem

systemctl enable nodegem_webapi
systemctl restart nodegem_webapi

echo 'Installation complete!'
