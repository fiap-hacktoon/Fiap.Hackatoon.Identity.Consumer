#See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8080
EXPOSE 8081
ENV ASPNETCORE_ENVIRONMENT=Development

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["ContactsConsult.Api/FIAP.TechChallenge.ContactsConsult.Api.csproj", "ContactsConsult.Api/"]
COPY ["ContactsConsult.Application/FIAP.TechChallenge.ContactsConsult.Application.csproj", "ContactsConsult.Application/"]
COPY ["ContactsConsult.Domain/FIAP.TechChallenge.ContactsConsult.Domain.csproj", "ContactsConsult.Domain/"]
COPY ["ContactsConsult.Infrastructure/FIAP.TechChallenge.ContactsConsult.Infrastructure.csproj", "ContactsConsult.Infrastructure/"]
RUN dotnet restore "./ContactsConsult.Api/FIAP.TechChallenge.ContactsConsult.Api.csproj"
COPY . .
WORKDIR "/src/ContactsConsult.Api"
RUN dotnet build "./FIAP.TechChallenge.ContactsConsult.Api.csproj" -c $BUILD_CONFIGURATION -o /app/build

FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./FIAP.TechChallenge.ContactsConsult.Api.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "FIAP.TechChallenge.ContactsConsult.Api.dll"]