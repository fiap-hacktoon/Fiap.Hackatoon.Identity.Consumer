#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8082
ENV ASPNETCORE_URLS=http://*:8082
ENV ASPNETCORE_ENVIRONMENT=Development


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

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.TechChallenge.UserHub.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.TechChallenge.UserHub.Api.dll"]