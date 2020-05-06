# MkeAlerts App

Interested in contributing? Let me know - cwilson at mkealerts.com.

## VS Code
* Open the root folder in VS Code
* To run the solution, in the VS Code terminal, type: dotnet run
* The site will be at:
  * http://localhost:5000
  * https://localhost:5001
* HMR (Hot Module Replacement) will run for VueJS. So as you edit Vue files, you'll see them reflected in the browser without refreshing.

## Visual Studio 2019
* Open the solution file
* To run the solution, press F5
* The site will be at:
  * http://localhost:5010
  * https://localhost:5011 -- I haven't actually been able to get this to work.
* HMR (Hot Module Replacement) will run for VueJS. So as you edit Vue files, you'll see them reflected in the browser without refreshing.

## Adding Domain Models
* Add data model class (in Models\Data)
* Add DTO model class (in Models\DTO)
* Add data model DbSet to EF schema (in Data\ApplicationDbContext)
* Add mapping relationship from data model to DTO model (in Models\Data\DataModelsProfile.cs)
* Add migration (run Add-Migration in Package Manager Console)
* Add validator (in Models\Data)
* Add validator singleton (in Startup)
* Add service (in Services\Data)
* Add transient service reference for service (in Startup)
* Add API controller (in Controllers\Data)

## Adding Vue Components
* Create the .vue file (in ClientApp\src\components)
* Update the "name" value in the .vue file
* Add the "import" to ClientApp\src\main.js
* Add a route, if needed, to ClientApp\src\main.js
* Add a Vue.Component call, if needed, to ClientApp\src\main.js

## Adding Vue CRUD Controllers for a new Domain Model
* Create a folder in ClientApp\src\components
* Copy boilerplate files (List, Fields, Add, Edit, Delete) from another controller
* Add nav items, if needed, to ClientApp\src\App.Vue
* Add imports for the files to ClientApp\src\main.js
* Register the fields component in ClientApp\src\main.js
* Add routing rules to ClientApp\src\main.js

## Tools Used in this Project
* ASP.NET Core 3.1
* Entity Framework Core
* AutoMapper
* FluentValidation
* Authentication with JWT
* OpenAPI 3 Docs with Swagger (via Swashbuckle)
* VueJS 2.x
* Vue CLI 3
* Vue Router
* Vuelidate
* Leaflet and vue2-leaflet
* Webpack
* Bootstrap and BootstrapVue 2.x


## Notes

The basic ASP.NET Core/VueJS skeleton app was based on: https://medium.com/software-ateliers/asp-net-core-vue-template-with-custom-configuration-using-cli-3-0-8288e18ae80b

When running Vue CLI commands, be in the "ClientApp" folder:
* npm install ...

When running .NET stuff, be in the root folder.
* dotnet run


## Hosting
* The .NET Core 3.1.3 Hosting Bundle for Windows: https://dotnet.microsoft.com/download/dotnet-core/thank-you/runtime-aspnetcore-3.1.3-windows-hosting-bundle-installer
* You may also want to turn on stdout logging in the published web.config. If you do this, be sure to create a Logs folder that the IIS user/application pool identity has access to.


# Entity Framework Core Commands
* Add EF migrations with: dotnet ef migrations add MigrationName
* Update database with: dotnet ef database update