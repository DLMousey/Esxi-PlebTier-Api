#ESXI Pleb Tier API
As many of us budget sysadmins/homelab owners will know - VMWare's fantastic ESXI hypervisor comes in a couple of different flavours, namely; 
- ~~Pleb tier~~ Free 
- Excuse me a second Mr. VMWare sales rep, i think my heart just stopped when you told me that price

The latter tier also includes a RESTful API built in, which can be used to build third party applications to administer the system, spinning up VMs,
changing settings and whatnot - this is a feature many developers, especially those interested in devops could make a lot of use of but alas - 
i don't have 4 and a half thousand dollars to throw at my hobby projects.

 This project aims to build a RESTful API that communicates with the host over SSH and executes commands using `esxcli`, in the beginning this is being built
 purely to spin up new VMs and power them on, which is all i'm initially interested in doing (because i like PaaS, but the price makes my wallet snap shut in self defence).
 As the project grows it'd be nice to be able to build in additional features for managing the system itself such as user auth, lockdown support and so on.
 
 # Developer setup
 - Clone this repo
 - Open `EsxiRestfulApi.sln` in rider/visual studio
 - Make a copy of `appsettings.Development.json.dist` called `appsettings.Development.json`
 - Substitute placeholders with your own database credentials
 - Create a database in postgres
 - Restore packages
 - Install dotnet-ef (version 3.0.0, should be done automatically when packages are restored)
 - `dotnet ef database update` to create the database structure
 - `dotnet run`
 
 

