#!/bin/bash

NODEGEM_HOST='https://cdn.nodegem.io'
NODEGEM_DOWNLOAD_PATH=releases/latest/client_service
NODEGEM_CLIENT_FOLDER_NAME=nodegem_client

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

mkdir -p $NODEGEM_CLIENT_FOLDER_NAME
tar -zxf $NODEGEM_FILE -C $NODEGEM_CLIENT_FOLDER_NAME/

systemctl stop nodegem

cp -f $NODEGEM_CLIENT_FOLDER_NAME/* /var/nodegem/
rm -rf $NODEGEM_CLIENT_FOLDER_NAME/ $NODEGEM_FILE
rm -rf /var/tmp/.net/Nodegem.ClientService

chown -R nodegemserviceuser:nodegemserviceuser /var/nodegem

systemctl start nodegem