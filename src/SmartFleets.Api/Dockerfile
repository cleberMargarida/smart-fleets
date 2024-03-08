#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["src/SmartFleets.Api/SmartFleets.Api.csproj", "src/SmartFleets.Api/"]
COPY ["src/ServiceModels/ServiceModels.csproj", "src/ServiceModels/"]
COPY ["src/SmartFleets.Infrastructure/SmartFleets.Infrastructure.csproj", "src/SmartFleets.Infrastructure/"]
COPY ["src/SmartFleets.Application/SmartFleets.Application.csproj", "src/SmartFleets.Application/"]
COPY ["src/SmartFleets.Domain/SmartFleets.Domain.csproj", "src/SmartFleets.Domain/"]
RUN dotnet restore "./src/SmartFleets.Api/./SmartFleets.Api.csproj"
COPY . .
WORKDIR "/src/src/SmartFleets.Api"
RUN dotnet build "./SmartFleets.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./SmartFleets.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "SmartFleets.Api.dll"]