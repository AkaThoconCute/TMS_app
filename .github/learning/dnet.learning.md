# .NET Learning

Command

How to create API .NET app:

- Create SLN file: mkdir ASP_app => cd ASP_app => dotnet new slnx
- List .NET app options: dotnet new list
- Create API app: dotnet new webapi --name ASP_app --output ASP_app
- App API app to SLN file: dotnet sln add ASP_app/ASP_app.csproj
- Create Gitignore file: dotnet new gitignore
- Create README file: echo "# My ASP Project" > README.md

Project .NET command:

- Pull packages: dotnet restore
- Add packages: dotnet add package Example
- Remove packages: dotnet remove package Example

EF .NET command:

- dotnet tool install --global dotnet-ef
- dotnet tool uninstall --global dotnet-ef
- dotnet tool list --global

- dotnet ef migrations list
- dotnet ef migrations add Starting
- dotnet ef migrations remove

- dotnet ef database update
- dotnet ef database drop

Global .NET command:

- Clean Nuget repo: dotnet nuget locals all --clear

Unit test command:

- dotnet new xunit -o ASP_app.Tests : Create a test project
- dotnet add ASP_app.Tests/ASP_app.Tests.csproj reference ASP_app/ASP_app.csproj : Test project reference API project
- dotnet sln ASP_app.slnx add ASP_app.Tests/ASP_app.Tests.csproj : Add Test project to solution file

Run projects:

- dotnet run --project ASP_app : Run the api project
- dotnet test : Run the test project
