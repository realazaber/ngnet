# See https://aka.ms/customizecontainer to learn how to customize your debug container and how Visual Studio uses this Dockerfile to build your images for faster debugging.

# This stage is used when running from VS in fast mode (Default for Debug configuration)
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
USER app
WORKDIR /app
EXPOSE 8085
EXPOSE 8086

ENV DB_CONNECTION="Server=host.docker.internal;Database=ngnet;User=root;Password=;Port=3306;"
ENV SMTP_EMAIL_ADDRESS="ngnet@ngnet.com"
ENV SMTP_PASSWORD="YourPassword"
ENV SMTP_CLIENT_PROVIDER="YourProvider"
ENV SMTP_MY_EMAIL="YourEmail"
ENV SMTP_PORT="25"
ENV LOGS_PATH="logs"

ENV DEMO_MODE="ENABLED"

ENV ADMIN_FIRSTNAME="Alexander"
ENV ADMIN_LASTNAME="Zaborski"
ENV ADMIN_EMAIL="azcooldude8@gmail.com"
ENV ADMIN_PROFILEIMG="profileimg"
ENV ADMIN_PASSWORD="Charlie69420*"

# This stage is used to build the service project
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
ARG BUILD_CONFIGURATION=Release
WORKDIR /src
COPY ["backend.csproj", "."]
RUN dotnet restore "./backend.csproj"
COPY . .
WORKDIR "/src/."
RUN dotnet build "./backend.csproj" -c $BUILD_CONFIGURATION -o /app/build

# This stage is used to publish the service project to be copied to the final stage
FROM build AS publish
ARG BUILD_CONFIGURATION=Release
RUN dotnet publish "./backend.csproj" -c $BUILD_CONFIGURATION -o /app/publish /p:UseAppHost=false

# This stage is used in production or when running from VS in regular mode (Default when not using the Debug configuration)
FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "backend.dll"]