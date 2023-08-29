# Stage 1
ARG ASPNET_IMAGE_TAG=6.0-bullseye-slim
ARG NODEJS_IMAGE_TAG=18-bullseye

FROM mcr.microsoft.com/dotnet/sdk:7.0 AS publish

WORKDIR /build

ENV DEBIAN_FRONTEND=noninteractive

COPY Dfe.ManageFreeSchoolProjects/. .

RUN dotnet restore Dfe.ManageFreeSchoolProjects
RUN dotnet build Dfe.ManageFreeSchoolProjects -c Release

RUN dotnet new tool-manifest
RUN dotnet tool install dotnet-ef

RUN mkdir -p /app/SQL
RUN dotnet ef migrations script --output /app/SQL/DbMigrationScript.sql --idempotent -p /build/Dfe.ManageFreeSchoolProjects.Data
RUN touch /app/SQL/DbMigrationScript.sql

RUN dotnet publish Dfe.ManageFreeSchoolProjects -c Release -o /app --no-build

COPY ./script/web-docker-entrypoint.sh /app/docker-entrypoint.sh

# Stage 2 - Build assets
FROM node:${NODEJS_IMAGE_TAG} as build
COPY --from=publish /app /app
WORKDIR /app/wwwroot
RUN npm install
RUN npm run build

# Stage 3 - Final
FROM "mcr.microsoft.com/dotnet/aspnet:${ASPNET_IMAGE_TAG}" AS final

ARG COMMIT_SHA

RUN apt-get update
RUN apt-get install unixodbc curl gnupg jq -y
RUN curl https://packages.microsoft.com/keys/microsoft.asc | apt-key add -
RUN curl https://packages.microsoft.com/config/debian/11/prod.list | tee /etc/apt/sources.list.d/msprod.list
RUN apt-get update
RUN ACCEPT_EULA=Y apt-get install msodbcsql18 mssql-tools18 -y

COPY --from=build /app /app
WORKDIR /app
RUN chmod +x ./docker-entrypoint.sh

EXPOSE 80/tcp
