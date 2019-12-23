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

if [ $self_hosted = true ]; then
  apt install -y unzip
fi

echo 'Downloading Nodegem service client...'
unameArch=$(uname -m)
case "$unameArch" in
    amd64)     NODEGEM_ARCH="64";;
    x86_64)    NODEGEM_ARCH="64";;
    aarch64)   NODEGEM_ARCH="64";;
    armv7l)    NODEGEM_ARCH="32";;
    * )        echo "Your Architecture '$unameArch' -> ITS NOT SUPPORTED.";;
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

if [ $self_hosted = true ]; then 
    systemctl unmask nodegem_webapi
    systemctl stop nodegem_webapi
    systemctl disable nodegem_webapi
fi

systemctl unmask nodegem
systemctl stop nodegem
systemctl disable nodegem

if [ $self_hosted = true ]; then
  tar -zxf $NODEGEM_API_FILE -C /var/nodegem/webapi
  unzip web_client.zip -d temp_web_client/
  cp -rp temp_web_client/build/* /var/nodegem/webapi/wwwroot/
  rm -rf temp_web_client web_client.zip
fi

tar -zxf $NODEGEM_FILE -C /var/nodegem/client
chown -R $SERVICE_USER:$SERVICE_USER /var/nodegem

rm -f $NODEGEM_FILE $NODEGEM_API_FILE

systemctl daemon-reload

if [ $self_hosted = true ]; then
  systemctl enable nodegem_webapi
  systemctl restart nodegem_webapi
fi

systemctl enable nodegem
systemctl restart nodegem

echo 'Installation complete!'