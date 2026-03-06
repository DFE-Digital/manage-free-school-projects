# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-azurelinux3.0@sha256:0456c8bc40434bba2a99949ecc104de1cdacf41cb7d6981339239b7f20ea5371 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 3001
EXPOSE 3000

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-azurelinux3.0@sha256:f528f787d6b658252bab774a3419287b9de5af0f9d9d5250c093caa9d432a68d AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["Dfe.ManageFreeSchoolProjects.API/Dfe.ManageFreeSchoolProjects.API.csproj", "Dfe.ManageFreeSchoolProjects.API/"]
COPY ["Dfe.ManageFreeSchoolProjects.API.Contracts/Dfe.ManageFreeSchoolProjects.API.Contracts.csproj", "Dfe.ManageFreeSchoolProjects.API.Contracts/"]
COPY ["Dfe.ManageFreeSchoolProjects.Data/.Data.csproj", "Dfe.ManageFreeSchoolProjects.Data/"]
COPY ["Dfe.ManageFreeSchoolProjects.UserContext/Dfe.ManageFreeSchoolProjects.UserContext.csproj", "Dfe.ManageFreeSchoolProjects.UserContext/"]
COPY ["Dfe.ManageFreeSchoolProjects.Logging/Dfe.ManageFreeSchoolProjects.Logging.csproj", "Dfe.ManageFreeSchoolProjects.Logging/"]
RUN dotnet restore "./Dfe.ManageFreeSchoolProjects.API/Dfe.ManageFreeSchoolProjects.API.csproj"
COPY ./Dfe.ManageFreeSchoolProjects/ .
WORKDIR "/src/Dfe.ManageFreeSchoolProjects.API"
RUN dotnet build "./Dfe.ManageFreeSchoolProjects.API.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Dfe.ManageFreeSchoolProjects.API.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Dfe.ManageFreeSchoolProjects.API.dll"]