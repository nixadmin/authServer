# authServer

Steps to run solution:
* Install .Net Core v.1.1.0 https://www.microsoft.com/net/core#macos
* Add valid connection string to PostgreSQL Database in to appsettings.json file in to PgSqlConnectionString variable
* Make sure DatabaseSettings looks like this. Application will migrate database tables
```JSON
  "DatabaseSettings": {
    "MigrateOnStartup": true,
    "MigrateOnStatupWithTestingData": false,
    "ConnectionName": "PgSqlConnectionString"
  },
```
* Make sure Facebook section also not empty
```JSON
  "Facebook": {
    "ClientId": "397479107258663",
    "Secret": "4d498e4d8d827605360cb08d90626ef1",
    "AuthScheme": "Identity.External",
    "DisplayName": "Facebook"
  },
```
* By default applciation will listen http://localhost:5000, make sure Authority variable targets itself
```JSON
  "ServerAuthentication": {
    "Authority": "http://localhost:5000",
    "RequireHttpsMetadata": false,
    "AutomaticAuthenticate": true,
    "AutomaticChallenge": true,
    "ApiName": "users"
  },
```
* Install Redis and specify host address in to Host varible
* Add *.pfx file with keys in to the application root folder. And specify file name and file password. 
* Now you should be able to run application. Navigate using cmd to foder where project.json is stored.
```cmd
dotnet build // will compile solution
dotnet run   // will start solution on http://localhost:5000
```
* Open in web browser http://localhost:5000/.well-known/openid-configuration and you should be able to see discovery document
* More information and samples about IdentityServer4 you can find here: http://docs.identityserver.io/en/release
