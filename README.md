# RCL CertificateBot

A Linux Daemon and Windows Service to automate the renewal of SSL/TLS certificates created in the RCL Portal. 

## How to use

- Install the Linux Daemon or Windows Service in the server running your websites
- Register and configure an **AAD Application** in Azure Active Directory for the service to use
- Every seven (7) days, the service will check for certificates about to expire and renew them automatically and also replace certificates in the server

## Read the documentation

You can read the detailed documentation to configure, install and test the service : 

[RCL CertificateBot Documentation](https://docs.rclapp.com/certbot/certbot.html)

## Contribute to this project

If you find a bug or want to add a new feature, we welcome contributions to this project.

This is how you can contribute :

- You need a basic understanding of Git and GitHub.com

- Open an [issue](https://github.com/rcl-ssl/RCL.CertificateBot/issues) describing what you want to do, such as fixing a bug or adding a new feature. Wait for approval before you invest much time

- Fork the repo of the **master** branch and create a new branch for your changes

- Submit a pull request (PR) to the **master** branch with your changes

- Respond to PR feedback

## RCL SDK

This application was built with the [RCL SDK](https://docs.rclapp.com/sdk/sdk.html)
