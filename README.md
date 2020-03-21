![GitHub](https://img.shields.io/github/license/mashape/apistatus.svg?style=popout)

**EveneumSample is a sample application that implements the Eveneum NuGet package, which is a simple, developer-friendly Event Store with snapshots, backed by Azure Cosmos DB.**

# Setup

## Cosmos
To run this sample, you will need access to a Cosmos database.  You can either allocate a DB in [Azure](https://azure.microsoft.com/en-us/services/cosmos-db/), or download the [Azure Cosmos Emulator](https://docs.microsoft.com/en-us/azure/cosmos-db/local-emulator) from Microsoft and run it locally.

## App settings

Edit the `appsettings.json` file in the solution and put your connection string in the `ConnectionString` setting.  It will likely be in the form

``` json
    "ConnectionString": "AccountEndpoint=https://localhost:8081/;AccountKey=C...="
```

After that, you should be able to run the application in either Visual Studio or Visual Studio Code.