# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0-azurelinux3.0 AS base

USER $APP_UID
WORKDIR /app
EXPOSE 5001
EXPOSE 5000
ARG PROJECT_NAME="Dfe.ManageFreeSchoolProjects"

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0-azurelinux3.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["${PROJECT_NAME}/${PROJECT_NAME}.csproj", "${PROJECT_NAME}/"]
COPY ["${PROJECT_NAME}.API.Contracts/${PROJECT_NAME}.API.Contracts.csproj", "${PROJECT_NAME}.API.Contracts/"]
COPY ["${PROJECT_NAME}.Logging/${PROJECT_NAME}.Logging.csproj", "${PROJECT_NAME}.Logging/"]
COPY ["${PROJECT_NAME}.UserContext/${PROJECT_NAME}.UserContext.csproj", "${PROJECT_NAME}.UserContext/"]
RUN dotnet restore "./${PROJECT_NAME}/${PROJECT_NAME}.csproj"
COPY ./${PROJECT_NAME}/ .
WORKDIR "/src/${PROJECT_NAME}"
RUN dotnet build "./${PROJECT_NAME}.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./${PROJECT_NAME}.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "${PROJECT_NAME}.dll"]