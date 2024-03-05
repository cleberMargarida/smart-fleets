#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/Ingestion.Api.Silo/Ingestion.Api.Silo.csproj", "src/Ingestion.Api.Silo/"]
COPY ["src/Ingestion.Api.GrainInterfaces/Ingestion.Api.GrainInterfaces.csproj", "src/Ingestion.Api.GrainInterfaces/"]
COPY ["src/ServiceModels/ServiceModels.csproj", "src/ServiceModels/"]
COPY ["src/Ingestion.Api.Grains/Ingestion.Api.Grains.csproj", "src/Ingestion.Api.Grains/"]
RUN dotnet restore "./src/Ingestion.Api.Silo/./Ingestion.Api.Silo.csproj"
COPY . .
WORKDIR "/src/src/Ingestion.Api.Silo"
RUN dotnet build "./Ingestion.Api.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./Ingestion.Api.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Ingestion.Api.Silo.dll"]
