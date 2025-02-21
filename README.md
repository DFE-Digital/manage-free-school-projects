# Manage Free School Projects Service
Internal service for managing free schools projects

## Requirements
- .NET 6.0
- NodeJS (for frontend build tools)
- [Docker Desktop](https://www.docker.com/products/docker-desktop/ "Download Docker Desktop")
- SQL Server Management Studio or Azure Data Studio

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
