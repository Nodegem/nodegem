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

NODEGEM_HOST='https://cdn.nodegem.io'

echo 'Nodegem credentials...'
read -p "Username: " nodegem_user
while true; do
    read -s -p "Password: " nodegem_password
    echo
    read -s -p "Password (again): " nodegem_password2
    echo
    [ "$nodegem_password" = "$nodegem_password2" ] && break
    echo "Please try again"
done

curl $NODEGEM_HOST/install/linux/nodegem.service.template -o nodegem.service.template

if [ $self_hosted = true ]; then
    extraArgs="-e http:\/\/localhost:5000"
    sed -e "s/\${username}/${nodegem_user}/" -e "s/\${password}/${nodegem_password}/" -e "s/\${extra}/${extraArgs}/" nodegem.service.template > nodegem.service
else
    extraArgs=""
    sed -e "s/\${username}/${nodegem_user}/" -e "s/\${password}/${nodegem_password}/" -e "s/\${extra}/${extraArgs}/" nodegem.service.template > nodegem.service
fi

rm nodegem.service.template

systemctl unmask nodegem
systemctl stop nodegem
systemctl disable nodegem

cp -f nodegem.service /etc/systemd/system
rm -f nodegem.service

systemctl daemon-reload

systemctl restart nodegem
systemctl enable nodegem

echo "Successfully changed user"