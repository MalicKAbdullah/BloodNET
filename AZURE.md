# Azure App Service Deployment Guide

## Overview

This guide covers deploying BloodNET to Azure App Service with Azure SQL Database or external database options.

## Cost Options

### Option 1: Azure Free Tier (Most Limited)
- **App Service**: Free (F1) - 60 CPU minutes/day, 1GB RAM, 1GB storage
- **Database**: 
  - Azure SQL Database Free (32MB) - Very limited
  - OR Supabase PostgreSQL (500MB) - Free
- **Total**: Free

### Option 2: Azure Basic Tier (Recommended)
- **App Service**: Basic (B1) - $13/month
- **Database**: Azure SQL Database Basic - $5/month
- **Total**: $18/month

### Option 3: Hybrid (Best Value)
- **App Service**: Free (F1) or Basic (B1)
- **Database**: Supabase PostgreSQL - Free (500MB)
- **Total**: Free or $13/month

---

## Prerequisites

1. **Azure Account**: Sign up at [portal.azure.com](https://portal.azure.com)
   - Free account includes $200 credit for 30 days
   - Credit card required but won't be charged for free tier

2. **Azure CLI** (optional but recommended):
   ```bash
   # Windows (PowerShell)
   winget install -e --id Microsoft.AzureCLI
   
   # macOS
   brew install azure-cli
   
   # Linux
   curl -sL https://aka.ms/InstallAzureCLIDeb | sudo bash
   ```

3. **.NET 8 SDK**: Already installed

---

## Deployment Method 1: Visual Studio (Easiest)

### Step 1: Install Azure Development Workload
- Open Visual Studio Installer
- Modify your installation
- Select "Azure development" workload
- Install

### Step 2: Create Azure Resources
1. Open Solution in Visual Studio
2. Right-click `BloodNET-Web` project → **Publish**
3. Select **Azure** → **Next**
4. Select **Azure App Service (Windows)** → **Next**
5. Click **Create New**
6. Configure:
   - **Name**: bloodnet-app (or your choice)
   - **Subscription**: Your Azure subscription
   - **Resource Group**: Create new → `BloodNET-RG`
   - **Hosting Plan**: Create new → Select **Free** tier
7. Click **Create**

### Step 3: Configure Database

#### Option A: Azure SQL Database
1. In Azure Portal, click **Create a resource** → **SQL Database**
2. Configure:
   - **Database name**: BloodNET
   - **Server**: Create new
   - **Compute + storage**: Basic (cheapest) or Free (if available)
   - **Backup storage redundancy**: Locally-redundant
3. Create the database
4. In the database, go to **Connection strings**
5. Copy the ADO.NET connection string

#### Option B: Supabase (Free Alternative)
1. Go to [supabase.com](https://supabase.com) → Create account
2. Create new project
3. Wait for provisioning
4. Go to **Settings** → **Database**
5. Copy the Connection String (ensure it's for .NET/C#)
6. Convert to Entity Framework format:
   ```
   Host=db.xxxxx.supabase.co;Database=postgres;Username=postgres;Password=yourpassword;Port=5432;SSL Mode=Require;Trust Server Certificate=true;
   ```

### Step 4: Configure Connection String in Azure
1. Go to Azure Portal → Your App Service
2. Navigate to **Configuration** → **Connection strings**
3. Add new connection string:
   - **Name**: `DefaultConnection`
   - **Value**: Your connection string from Step 3
   - **Type**: `SQLAzure` (for Azure SQL) or `Custom` (for PostgreSQL)
4. Click **Save**

### Step 5: Deploy
1. In Visual Studio, in the Publish profile, click **Publish**
2. Wait for deployment to complete
3. Application will open in browser automatically

### Step 6: Run Database Migrations

**Option A: From Visual Studio (requires VPN/firewall configured)**
```powershell
# In Package Manager Console
Update-Database
```

**Option B: Add automatic migrations** (Recommended)

Add to `Program.cs` before `app.Run()`:
```csharp
if (!app.Environment.IsDevelopment())
{
    using (var scope = app.Services.CreateScope())
    {
        var services = scope.ServiceProvider;
        try
        {
            var context = services.GetRequiredService<ApplicationDbContext>();
            context.Database.Migrate();
        }
        catch (Exception ex)
        {
            var logger = services.GetRequiredService<ILogger<Program>>();
            logger.LogError(ex, "An error occurred while migrating the database.");
        }
    }
}
```

Then re-publish.

---

## Deployment Method 2: Azure CLI

### Step 1: Login to Azure
```bash
az login
```

### Step 2: Create Resource Group
```bash
az group create --name BloodNET-RG --location eastus
```

### Step 3: Create App Service Plan
```bash
# Free tier
az appservice plan create --name BloodNET-Plan --resource-group BloodNET-RG --sku FREE

# Or Basic tier (recommended)
az appservice plan create --name BloodNET-Plan --resource-group BloodNET-RG --sku B1
```

### Step 4: Create Web App
```bash
az webapp create --resource-group BloodNET-RG --plan BloodNET-Plan --name bloodnet-app --runtime "DOTNET|8.0"
```

### Step 5: Create Azure SQL Database (Optional)
```bash
# Create SQL Server
az sql server create --name bloodnet-server --resource-group BloodNET-RG --location eastus --admin-user sqladmin --admin-password "YourStrongPassword123!"

# Create Database (Basic tier)
az sql db create --resource-group BloodNET-RG --server bloodnet-server --name BloodNET --service-objective Basic

# Configure firewall to allow Azure services
az sql server firewall-rule create --resource-group BloodNET-RG --server bloodnet-server --name AllowAzureServices --start-ip-address 0.0.0.0 --end-ip-address 0.0.0.0
```

### Step 6: Configure Connection String
```bash
# For Azure SQL
az webapp config connection-string set --resource-group BloodNET-RG --name bloodnet-app --settings DefaultConnection="Server=tcp:bloodnet-server.database.windows.net,1433;Database=BloodNET;User ID=sqladmin;Password=YourStrongPassword123!;Encrypt=True;TrustServerCertificate=False;Connection Timeout=30;" --connection-string-type SQLAzure

# For PostgreSQL/Supabase
az webapp config connection-string set --resource-group BloodNET-RG --name bloodnet-app --settings DefaultConnection="Host=xxx;Database=xxx;Username=xxx;Password=xxx;Port=5432;SSL Mode=Require" --connection-string-type Custom
```

### Step 7: Build and Deploy
```bash
cd BloodNET-Web

# Publish locally
dotnet publish -c Release -o ./publish

# Create zip file
cd publish
zip -r ../deploy.zip .
cd ..

# Deploy to Azure
az webapp deploy --resource-group BloodNET-RG --name bloodnet-app --src-path deploy.zip --type zip

# Or use deployment from GitHub (recommended)
az webapp deployment source config --name bloodnet-app --resource-group BloodNET-RG --repo-url https://github.com/yourusername/BloodNET --branch main --manual-integration
```

---

## Deployment Method 3: GitHub Actions (Recommended for CI/CD)

### Step 1: Create Azure Resources
Follow steps in Method 2 (Azure CLI) or use Azure Portal.

### Step 2: Get Publish Profile
```bash
az webapp deployment list-publishing-profiles --name bloodnet-app --resource-group BloodNET-RG --xml
```

Copy the output.

### Step 3: Add GitHub Secret
1. Go to your GitHub repository → **Settings** → **Secrets and variables** → **Actions**
2. Click **New repository secret**
3. Name: `AZURE_WEBAPP_PUBLISH_PROFILE`
4. Value: Paste the XML from Step 2
5. Click **Add secret**

### Step 4: Create GitHub Actions Workflow

Create `.github/workflows/azure-deploy.yml`:

```yaml
name: Deploy to Azure App Service

on:
  push:
    branches: [ main ]
  workflow_dispatch:

jobs:
  build-and-deploy:
    runs-on: ubuntu-latest
    
    steps:
    - uses: actions/checkout@v3
    
    - name: Setup .NET
      uses: actions/setup-dotnet@v3
      with:
        dotnet-version: '8.0.x'
    
    - name: Restore dependencies
      run: dotnet restore
      
    - name: Build
      run: dotnet build --configuration Release --no-restore
      
    - name: Publish
      run: dotnet publish BloodNET-Web/BloodNET-Web.csproj -c Release -o ${{env.DOTNET_ROOT}}/myapp
      
    - name: Deploy to Azure Web App
      uses: azure/webapps-deploy@v2
      with:
        app-name: 'bloodnet-app'
        publish-profile: ${{ secrets.AZURE_WEBAPP_PUBLISH_PROFILE }}
        package: ${{env.DOTNET_ROOT}}/myapp
```

### Step 5: Push and Deploy
```bash
git add .github/workflows/azure-deploy.yml
git commit -m "Add Azure deployment workflow"
git push
```

GitHub Actions will automatically deploy on every push to main branch.

---

## Configuration Tips

### Environment Variables
In Azure Portal → Configuration → Application settings:

```
ASPNETCORE_ENVIRONMENT = Production
ASPNETCORE_URLS = http://+:80
```

### Enable Logging
```bash
az webapp log config --name bloodnet-app --resource-group BloodNET-RG --application-logging filesystem --level information
```

View logs:
```bash
az webapp log tail --name bloodnet-app --resource-group BloodNET-RG
```

### Custom Domain (Optional)
1. Purchase domain
2. In Azure Portal → Custom domains
3. Add custom domain
4. Configure DNS records
5. Enable HTTPS (free with Azure)

---

## Database Options Comparison

| Option | Cost | Size | Features | Pros | Cons |
|--------|------|------|----------|------|------|
| Azure SQL Free | Free | 32MB | Basic | Integrated | Very limited size |
| Azure SQL Basic | $5/mo | 2GB | Full SQL Server | Reliable, scalable | Paid |
| Supabase | Free | 500MB | PostgreSQL + extras | Good size, extras | Requires PG conversion |
| Railway | $5 credit/mo | 1GB | PostgreSQL | Easy, includes hosting | Usage-based |

---

## PostgreSQL Conversion (for Supabase)

If using Supabase or other PostgreSQL options:

1. **Add Npgsql package**:
   ```bash
   dotnet add BloodNET-Web package Npgsql.EntityFrameworkCore.PostgreSQL
   ```

2. **Update Program.cs**:
   ```csharp
   // Replace UseSqlServer with UseNpgsql
   builder.Services.AddDbContext<ApplicationDbContext>(options =>
       options.UseNpgsql(connectionString));
   ```

3. **Create new migrations**:
   ```bash
   dotnet ef migrations add InitialPostgreSQL
   ```

See `POSTGRESQL.md` for detailed conversion instructions.

---

## Monitoring

### Application Insights (Recommended)
1. In Azure Portal → Application Insights → Create
2. Connect to your App Service
3. View:
   - Performance metrics
   - Failed requests
   - Exceptions
   - Custom events

### Basic Monitoring (Free)
Azure Portal → Your App Service → Monitoring:
- Metrics (CPU, Memory, Response time)
- Logs (Application logs, HTTP logs)
- Alerts (Set up email alerts)

---

## Scaling

### Vertical Scaling (Size)
```bash
az appservice plan update --name BloodNET-Plan --resource-group BloodNET-RG --sku B2
```

### Horizontal Scaling (Instances)
```bash
az appservice plan update --name BloodNET-Plan --resource-group BloodNET-RG --number-of-workers 2
```

---

## Troubleshooting

### App Won't Start
- Check logs: Azure Portal → Log stream
- Verify connection string is set correctly
- Ensure .NET 8.0 is selected in Configuration

### Database Connection Failed
- Check firewall rules in Azure SQL
- Verify connection string format
- Test connection locally with same string

### Deployment Failed
- Check build logs in Azure Portal
- Verify all NuGet packages are restored
- Check for missing files or dependencies

---

## Cost Management

### Free Tier Limitations
- 60 CPU minutes/day (enough for testing)
- App sleeps after 20 min of inactivity
- Limited to 1GB storage
- Limited to 165MB/day bandwidth

### Cost-Saving Tips
1. Use Supabase for database (free 500MB)
2. Start with Free tier, upgrade only if needed
3. Use GitHub Actions for CI/CD (free for public repos)
4. Monitor usage in Cost Management

---

## Security Best Practices

1. **Never commit secrets**: Use Azure Key Vault or App Settings
2. **Enable HTTPS**: Always use HTTPS in production
3. **Configure authentication**: Enable Azure AD if needed
4. **Set up firewall**: Restrict database access
5. **Regular updates**: Keep packages updated
6. **Monitor security**: Use Azure Security Center

---

## Next Steps

1. ✅ Create Azure account
2. ✅ Choose database option (Azure SQL or Supabase)
3. ✅ Deploy application (Visual Studio, CLI, or GitHub Actions)
4. ✅ Configure connection string
5. ✅ Run database migrations
6. ✅ Test application
7. ✅ Set up monitoring
8. ✅ Configure custom domain (optional)

---

## Support

- **Azure Documentation**: https://docs.microsoft.com/azure
- **Azure Support**: Available in Azure Portal
- **Community**: Stack Overflow, Azure Forums

Let me know if you need help with any step!
