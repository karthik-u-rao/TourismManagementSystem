# Deployment Guide - Tourism Management System

This guide provides step-by-step instructions for deploying the Tourism Management System to various environments.

## Table of Contents

- [Prerequisites](#prerequisites)
- [Local Development Setup](#local-development-setup)
- [Production Deployment](#production-deployment)
- [Azure App Service Deployment](#azure-app-service-deployment)
- [IIS Deployment](#iis-deployment)
- [Environment Configuration](#environment-configuration)
- [Database Setup](#database-setup)
- [Security Considerations](#security-considerations)

## Prerequisites

### System Requirements
- .NET 8.0 Runtime or SDK
- SQL Server 2019+ or Azure SQL Database
- IIS 10+ (for Windows deployment)
- Windows Server 2019+ or Linux (Ubuntu 20.04+)

### Development Requirements
- Visual Studio 2022 or VS Code
- .NET 8.0 SDK
- SQL Server (LocalDB for development)
- Git

## Local Development Setup

### 1. Clone Repository
```bash
git clone https://github.com/karthik-u-rao/TourismManagementSystem.git
cd TourismManagementSystem
```

### 2. Configure Database
```json
// appsettings.json
{
  "ConnectionStrings": {
    "TourismDb": "Server=(localdb)\\MSSQLLocalDB;Database=TourismDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
  }
}
```

### 3. Run Migrations
```bash
dotnet ef database update --project Tourism.DataAccess --startup-project TourismManagementSystem
```

### 4. Start Application
```bash
dotnet run --project TourismManagementSystem
```

## Production Deployment

### 1. Publish Application
```bash
# Create production build
dotnet publish TourismManagementSystem -c Release -o ./publish

# Or with specific runtime
dotnet publish TourismManagementSystem -c Release -r win-x64 --self-contained false -o ./publish
```

### 2. Production Configuration
```json
// appsettings.Production.json
{
  "ConnectionStrings": {
    "TourismDb": "Server=your-production-server;Database=TourismDb;User Id=your-user;Password=your-password;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "yourdomain.com"
}
```

## Azure App Service Deployment

### Method 1: Visual Studio Deployment

1. Right-click on TourismManagementSystem project
2. Select "Publish"
3. Choose "Azure App Service"
4. Configure deployment settings
5. Publish

### Method 2: Azure CLI Deployment

```bash
# Login to Azure
az login

# Create resource group
az group create --name TourismApp-rg --location "East US"

# Create App Service plan
az appservice plan create --name TourismApp-plan --resource-group TourismApp-rg --sku B1

# Create web app
az webapp create --name your-tourism-app --resource-group TourismApp-rg --plan TourismApp-plan --runtime "DOTNET|8.0"

# Deploy application
az webapp deployment source config-zip --resource-group TourismApp-rg --name your-tourism-app --src publish.zip
```

### Azure Configuration

```bash
# Set connection string
az webapp config connection-string set --resource-group TourismApp-rg --name your-tourism-app --settings TourismDb="Server=your-azure-sql-server.database.windows.net;Database=TourismDb;User Id=your-user;Password=your-password;" --connection-string-type SQLAzure

# Set environment variables
az webapp config appsettings set --resource-group TourismApp-rg --name your-tourism-app --settings ASPNETCORE_ENVIRONMENT=Production
```

## IIS Deployment

### 1. Install Prerequisites
- ASP.NET Core Hosting Bundle for .NET 8.0
- IIS with ASP.NET Core Module

### 2. Create IIS Application

```powershell
# Import WebAdministration module
Import-Module WebAdministration

# Create application pool
New-WebAppPool -Name "TourismApp" -Force
Set-ItemProperty -Path "IIS:\AppPools\TourismApp" -Name "processModel.identityType" -Value "ApplicationPoolIdentity"
Set-ItemProperty -Path "IIS:\AppPools\TourismApp" -Name "managedRuntimeVersion" -Value ""

# Create website
New-Website -Name "TourismApp" -ApplicationPool "TourismApp" -PhysicalPath "C:\inetpub\wwwroot\TourismApp" -Port 80
```

### 3. Configure web.config

```xml
<?xml version="1.0" encoding="utf-8"?>
<configuration>
  <location path="." inheritInChildApplications="false">
    <system.webServer>
      <handlers>
        <add name="aspNetCore" path="*" verb="*" modules="AspNetCoreModuleV2" resourceType="Unspecified" />
      </handlers>
      <aspNetCore processPath="dotnet" 
                  arguments=".\TourismManagementSystem.dll" 
                  stdoutLogEnabled="false" 
                  stdoutLogFile=".\logs\stdout" 
                  hostingModel="inprocess" />
    </system.webServer>
  </location>
</configuration>
```

## Environment Configuration

### Development (appsettings.Development.json)
```json
{
  "ConnectionStrings": {
    "TourismDb": "Server=(localdb)\\MSSQLLocalDB;Database=TourismDb;Trusted_Connection=True;MultipleActiveResultSets=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  }
}
```

### Production (appsettings.Production.json)
```json
{
  "ConnectionStrings": {
    "TourismDb": "Server=prod-server;Database=TourismDb;User Id=app_user;Password=secure_password;TrustServerCertificate=true;"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Warning",
      "Microsoft.AspNetCore": "Error"
    }
  },
  "AllowedHosts": "yourdomain.com,www.yourdomain.com"
}
```

## Database Setup

### 1. Create Production Database

```sql
-- Create database
CREATE DATABASE TourismDb;

-- Create application user
CREATE LOGIN [tourism_app] WITH PASSWORD = 'SecurePassword123!';
USE TourismDb;
CREATE USER [tourism_app] FOR LOGIN [tourism_app];
ALTER ROLE db_datareader ADD MEMBER [tourism_app];
ALTER ROLE db_datawriter ADD MEMBER [tourism_app];
ALTER ROLE db_ddladmin ADD MEMBER [tourism_app];
```

### 2. Run Migrations in Production

```bash
# Set production connection string
export ConnectionStrings__TourismDb="Server=prod-server;Database=TourismDb;User Id=tourism_app;Password=SecurePassword123!;"

# Run migrations
dotnet ef database update --project Tourism.DataAccess --startup-project TourismManagementSystem
```

## Security Considerations

### 1. SSL/TLS Configuration

```csharp
// Program.cs - Production
if (!app.Environment.IsDevelopment())
{
    app.UseHsts();
    app.UseHttpsRedirection();
}
```

### 2. Security Headers

```csharp
app.Use(async (context, next) =>
{
    context.Response.Headers.Add("X-Content-Type-Options", "nosniff");
    context.Response.Headers.Add("X-Frame-Options", "DENY");
    context.Response.Headers.Add("X-XSS-Protection", "1; mode=block");
    context.Response.Headers.Add("Referrer-Policy", "strict-origin-when-cross-origin");
    await next();
});
```

### 3. Connection String Security

Use environment variables or Azure Key Vault for sensitive configuration.

## Deployment Checklist

### Pre-Deployment
- [ ] Code review completed
- [ ] All tests passing
- [ ] Database backup taken
- [ ] Configuration verified
- [ ] SSL certificate ready

### Deployment
- [ ] Application published
- [ ] Database migrations applied
- [ ] Configuration files updated
- [ ] SSL certificate installed
- [ ] Health checks passing

### Post-Deployment
- [ ] Application accessible
- [ ] Database connectivity verified
- [ ] Admin login working
- [ ] Sample booking flow tested
- [ ] Error logs checked

## Troubleshooting

### Common Issues

1. Connection String Issues
   ```bash
   # Test connection
   sqlcmd -S server-name -d TourismDb -U username -P password -Q "SELECT 1"
   ```

2. Migration Issues
   ```bash
   # Reset migrations
   dotnet ef database drop --project Tourism.DataAccess --startup-project TourismManagementSystem
   dotnet ef database update --project Tourism.DataAccess --startup-project TourismManagementSystem
   ```