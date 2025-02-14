# Manage Free School Projects Service
Internal service for managing free schools projects

## Requirements
- .NET 6.0
- NodeJS (for frontend build tools)

## Development Setup

- Run `npm install; npm run build` from the `Dfe.ManageFreeSchoolProjects/wwwroot` directory to build the styles.
- Run `dotnet restore` from the `Dfe.ManageFreeSchoolProjects` project to restore dependencies.
- Run `dotnet run` from the `Dfe.ManageFreeSchoolProjects` project to run the application.

## SQL Database Setup

- Log into the `master` database as a Server Administrator
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
