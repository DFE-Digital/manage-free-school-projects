
# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0@sha256:b473b5d596c09fd55bb41f615623d349712013c1c6f4dfff9db741ac8dff36ec AS base
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:a9330090b730c2abde8f3f43b3d1e24e4b8cba028de7bab1a7fdcd50cb7a3b7e AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src

# Copy project files in correct order (dependencies first)
COPY ["/Dfe.ManageFreeSchoolProjects.API.Contracts/Dfe.ManageFreeSchoolProjects.API.Contracts.csproj", "Dfe.ManageFreeSchoolProjects.API.Contracts/"]
COPY ["/Dfe.ManageFreeSchoolProjects.Data/Dfe.ManageFreeSchoolProjects.Data.csproj", "Dfe.ManageFreeSchoolProjects.Data/"]
RUN dotnet restore "Dfe.ManageFreeSchoolProjects.Data/Dfe.ManageFreeSchoolProjects.Data.csproj"
COPY . .
WORKDIR "/src/Dfe.ManageFreeSchoolProjects.Data"

## Run migrations
RUN dotnet tool install --version 8.0.14 --global dotnet-ef
ENV PATH="$PATH:/root/.dotnet/tools"
ENTRYPOINT dotnet-ef database update --connection "$CONNECTION_STRING"