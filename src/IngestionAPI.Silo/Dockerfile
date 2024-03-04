#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/IngestionAPI.Silo/IngestionAPI.Silo.csproj", "src/IngestionAPI.Silo/"]
COPY ["src/IngestionAPI.GrainInterfaces/IngestionAPI.GrainInterfaces.csproj", "src/IngestionAPI.GrainInterfaces/"]
COPY ["src/ServiceModels/ServiceModels.csproj", "src/ServiceModels/"]
COPY ["src/IngestionAPI.Grains/IngestionAPI.Grains.csproj", "src/IngestionAPI.Grains/"]
RUN dotnet restore "./src/IngestionAPI.Silo/./IngestionAPI.Silo.csproj"
COPY . .
WORKDIR "/src/src/IngestionAPI.Silo"
RUN dotnet build "./IngestionAPI.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./IngestionAPI.Silo.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "IngestionAPI.Silo.dll"]