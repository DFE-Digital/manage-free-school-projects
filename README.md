# Manage Free School Projects Service
Internal service for managing free schools projects

## Requirements
- .NET 8.0
- NodeJS (for frontend build tools)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/ "Download Docker Desktop")
- SQL Server Management Studio or Azure Data Studio

## Development Setup - Docker compose :star2: NEW :star2: 
 
1 - Bring up the dev stack:
  - Navigate to the root of the project `managefreeschoolprojects`
  - Run the command `docker-compose -f Stack/docker-compose.yml up -d --build`. 
  
        - This will bring up the sql sever with a username and password set.
        - Create an empty database.
        - Apply migrations.

2 - Open the project in Visual studio :rocket:
3 - Run `npm install; npm run build` from the `Dfe.ManageFreeSchoolProjects/wwwroot` directory to build the styles.

# Adding migrations

- In command line navigate to Dfe.ManageFreeSchoolProjects.Data
- Issue the command `dotnet ef migrations add <MigrationName>`
- After making changes to run the migration run the command `dotnet ef database update --context MfspContext --connection "<db connection string>"`

## Development Setup

- Run `npm install; npm run build` from the `Dfe.ManageFreeSchoolProjects/wwwroot` directory to build the styles.
- Run `dotnet restore` from the `Dfe.ManageFreeSchoolProjects` project to restore dependencies.
- Run `dotnet run` from the `Dfe.ManageFreeSchoolProjects` project to run the application.

## Docker Setup

In a terminal:
- Run `docker pull mcr.microsoft.com/azure-sql-edge:latest\` to pull the latest version of Azure SQL Edge.
- Run `docker run --cap-add SYS_PTRACE -e "ACCEPT_EULA=Y" -e "MSSQL_SA_PASSWORD=INSERT_CHOSEN_PASSWORD" -p 1433:1433 --name azuresqledgeMFSP -d mcr.microsoft.com/azure-sql-edge` and replace the password with one of your choice.

## SQL Database Setup

- Connect to `127.0.0.1,1433` and log into the `master` database as a Server Administrator with the password you chose.
- Create a login for the app: `CREATE LOGIN [mfsp] WITH PASSWORD = 'EnterAStrongPasswordHere&';`
- Log into the specific database used by this app, and create the schema, user and permissions:
```sql
CREATE USER [mfsp] FOR LOGIN [mfsp] WITH DEFAULT_SCHEMA = [dbo];
GRANT CREATE TABLE TO [mfsp];
CREATE SCHEMA [mfsp];
GRANT CONTROL ON SCHEMA::[dbo] TO [mfsp];
GRANT CONTROL ON SCHEMA::[mfsp] TO [mfsp];
```

## Linting Sonar rules

Include the following extension in your IDE installation: [SonarQube for IDE](https://marketplace.visualstudio.com/items?itemName=SonarSource.sonarlint-vscode)

Update your [settings.json file](https://code.visualstudio.com/docs/getstarted/settings#_settings-json-file) to include the following

```json
"sonarlint.connectedMode.connections.sonarcloud": [   
    {
        "connectionId": "DfE",
        "organizationKey": "dfe-digital",
        "disableNotifications": false
    }   
]
```

Then follow [these steps](https://youtu.be/m8sAdYCIWhY) to connect to the SonarCloud instance.
