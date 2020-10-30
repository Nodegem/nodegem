---
description: The messenger
---

# Web API

## Quick Deploy

[![Deploy](https://www.herokucdn.com/deploy/button.svg)](https://heroku.com/deploy?template=https://github.com/Nodegem/nodegem)[![Deploy](https://cdn.nodegem.io/assets/linode-button.svg)](https://cloud.linode.com/linodes/create?type=One-Click&subtype=Community%20StackScripts&stackScriptID=625535)

## Windows

#### Instructions

Download the **Installer Script**, open Powershell as Administrator \(right-click Powershell and click 'Run as Adminstrator'\) and navigate to where you installed the script. Run the command `./install_nodegem.ps1 -SelfHosted 1` \(you may need to run the command `Set-ExecutionPolicy Unrestricted -Process Local` \) and it will prompt you for your **Nodegem** username and password.  
  
To test to see if the installation was successful you can visit [http://localhost:5000](http://localhost:5000) and you should see the login page.

* [Nodegem Installer Script](https://cdn.nodegem.io/install/windows/scripts/install_nodegem.ps1)
* [Nodegem Updater Script](https://cdn.nodegem.io/install/windows/scripts/update_nodegem.ps1) \(A script to quickly update the nodegem client\)
* [Nodegem Change User Script](https://cdn.nodegem.io/install/windows/scripts/change_user.ps1) \(Used to change the currently logged in user within the Client Service\)

## Linux

#### Instructions

Download the **Installer Script**, open a terminal, and run the command `sudo ./install_nodegem.sh --self-hosted=true` which will prompt you for your **Nodegem** username and password.  
  
To test to see if the installation was successful you can visit [http://localhost:5000](http://localhost:5000) and you should see the login page.

* [Nodegem Installer Script](https://cdn.nodegem.io/install/linux/install_nodegem.sh)
* [Nodegem Updater Script](https://cdn.nodegem.io/install/linux/update_nodegem.sh) \(A script to quickly update the nodegem client\)
* [Nodegem Change User Script](https://cdn.nodegem.io/install/linux/change_user.sh) \(Used to change the currently logged in user within the Client Service\)

