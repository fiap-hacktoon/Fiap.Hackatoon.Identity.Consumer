# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER $APP_UID
WORKDIR /app
EXPOSE 8080
EXPOSE 8082


# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["UserHub.Api/FIAP.TechChallenge.UserHub.Api.csproj", "UserHub.Api/"]
COPY ["UserHub.Application/FIAP.TechChallenge.UserHub.Application.csproj", "UserHub.Application/"]
COPY ["UserHub.Domain/FIAP.TechChallenge.UserHub.Domain.csproj", "UserHub.Domain/"]
COPY ["UserHub.Infrastructure/FIAP.TechChallenge.UserHub.Infrastructure.csproj", "UserHub.Infrastructure/"]
RUN dotnet restore "./UserHub.Api/FIAP.TechChallenge.UserHub.Api.csproj"
COPY . .
WORKDIR "/src/UserHub.Api"
RUN dotnet build "./FIAP.TechChallenge.UserHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.TechChallenge.UserHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.TechChallenge.UserHub.Api.dll"]