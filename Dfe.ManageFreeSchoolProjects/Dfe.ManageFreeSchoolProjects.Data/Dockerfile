
# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0 AS base
USER $APP_UID
WORKDIR /app
ARG PROJECT_NAME="Dfe.ManageFreeSchoolProjects"

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files in correct order (dependencies first)
COPY ["${PROJECT_NAME}.API.Contracts/${PROJECT_NAME}.API.Contracts.csproj", "${PROJECT_NAME}.API.Contracts/"]
COPY ["${PROJECT_NAME}.Data/${PROJECT_NAME}.Data.csproj", "${PROJECT_NAME}.Data/"]
RUN dotnet restore "${PROJECT_NAME}.Data/${PROJECT_NAME}.Data.csproj"
COPY ./${PROJECT_NAME}/ .
WORKDIR "/src/${PROJECT_NAME}.Data"

## Run migrations
RUN dotnet tool install --version 8.0.14 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --connection "$CONNECTION_STRING"