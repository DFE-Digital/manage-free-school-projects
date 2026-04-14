
# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/runtime:8.0@sha256:d0a2313ac5fbf1dfda2bf57e63a431010ae030dc3b9d7fc0fd1cff163b49b0e6 AS base
USER $APP_UID
WORKDIR /app

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0@sha256:ec47373b40fc22a31c891c5be8df83f8cbadc24e785b10899545e33a1a1c2183 AS build
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