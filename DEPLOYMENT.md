# BloodNET - Free Deployment Guide

## Application Overview

BloodNET is an ASP.NET Core 8.0 web application for blood donation management with the following technical stack:

- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server (currently configured for local SQL Server Express)
- **Features**: 
  - ASP.NET Identity for authentication
  - SignalR for real-time notifications
  - Entity Framework Core for data access
  - Dapper for some direct SQL queries

## Database Schema

The application uses the following tables:
- `AspNetUsers` and related Identity tables (managed by EF Core)
- `BloodDonors` - Donor information
- `BloodRequests` - Blood donation requests
- `Contacts` - Contact form submissions
- `Donation` - Donation records linking donors and requests
- `Respondants` - Response tracking

## Free Deployment Options

### Option 1: Azure App Service (Recommended for .NET)

**Free Tier Limits**: 60 CPU minutes/day, 1 GB RAM, 1 GB storage

#### Prerequisites
- Azure account (free tier available)
- Azure CLI or Azure Portal access

#### Database Options for Azure
1. **Azure SQL Database (Free Tier - 32MB)** - Very limited but free
2. **Azure Database for PostgreSQL** - Has free tier option
3. **Supabase PostgreSQL** - Free tier with 500MB database

#### Steps to Deploy:

1. **Create Azure App Service**
   ```bash
   az webapp create --name bloodnet-app --resource-group YourResourceGroup --plan YourAppServicePlan --runtime "DOTNET|8.0"
   ```

2. **Configure Connection String**
   - Navigate to Azure Portal → Your App Service → Configuration → Connection Strings
   - Add connection string with name `DefaultConnection`
   - For Azure SQL: `Server=tcp:yourserver.database.windows.net,1433;Database=BloodNET;User ID=yourusername;Password=yourpassword;Encrypt=True;TrustServerCertificate=False;`

3. **Deploy using Visual Studio**
   - Right-click project → Publish → Azure → Azure App Service
   - Select your app service
   - Click Publish

4. **Deploy using Azure CLI**
   ```bash
   cd BloodNET-Web
   dotnet publish -c Release
   az webapp deploy --resource-group YourResourceGroup --name bloodnet-app --src-path ./bin/Release/net8.0/publish.zip --type zip
   ```

5. **Run Database Migrations**
   ```bash
   # From Package Manager Console in Visual Studio
   Update-Database
   
   # Or using dotnet CLI
   dotnet ef database update
   ```

**Cost**: Free tier available, upgrade options if needed

---

### Option 2: Railway (Recommended - Easiest Setup)

**Free Tier**: $5 credit/month, includes PostgreSQL database

Railway is excellent for .NET applications and provides:
- Automatic HTTPS
- Automatic deployments from GitHub
- Built-in PostgreSQL database
- Simple configuration

#### Steps to Deploy:

1. **Sign up at [Railway.app](https://railway.app)** using GitHub

2. **Create New Project** → Deploy from GitHub repo

3. **Add PostgreSQL Database**
   - Click "New" → Database → PostgreSQL
   - Railway will automatically create the database

4. **Configure Application**
   - Railway will auto-detect .NET application
   - Add environment variable: `ConnectionStrings__DefaultConnection` with PostgreSQL connection string
   - Railway provides the connection string in database settings

5. **Update appsettings.json for PostgreSQL** (see PostgreSQL section below)

6. **Deploy**
   - Push to GitHub
   - Railway automatically builds and deploys

**Pros**: 
- Very easy setup
- Includes database
- Automatic HTTPS
- GitHub integration

**Cons**: 
- $5 credit runs out based on usage
- Need to convert from SQL Server to PostgreSQL

---

### Option 3: Render

**Free Tier**: 750 hours/month, includes PostgreSQL

#### Steps to Deploy:

1. **Sign up at [Render.com](https://render.com)**

2. **Create PostgreSQL Database**
   - Dashboard → New → PostgreSQL
   - Free tier: 90 days then expires (can create new one)

3. **Create Web Service**
   - Dashboard → New → Web Service
   - Connect GitHub repository
   - Environment: Docker
   - Use Dockerfile (see below)

4. **Configure Environment Variables**
   - Add `ConnectionStrings__DefaultConnection` with PostgreSQL connection
   - Add `ASPNETCORE_ENVIRONMENT=Production`

5. **Deploy**
   - Render will automatically build and deploy

**Pros**: 
- Good free tier
- Includes PostgreSQL
- Automatic SSL

**Cons**: 
- Free tier sleeps after 15 minutes of inactivity
- Database expires after 90 days on free tier

---

### Option 4: Fly.io

**Free Tier**: 3 shared VMs, 3GB storage

#### Steps to Deploy:

1. **Install Fly CLI**
   ```bash
   curl -L https://fly.io/install.sh | sh
   ```

2. **Login and Initialize**
   ```bash
   flyctl auth login
   cd BloodNET-Web
   flyctl launch
   ```

3. **Create PostgreSQL Database**
   ```bash
   flyctl postgres create
   flyctl postgres attach --app bloodnet-app yourpostgresapp
   ```

4. **Deploy**
   ```bash
   flyctl deploy
   ```

**Pros**: 
- Good free tier
- Fast global deployment
- Includes PostgreSQL

**Cons**: 
- Requires credit card for verification
- CLI-based setup

---

## Converting from SQL Server to PostgreSQL

Many free hosting platforms offer PostgreSQL instead of SQL Server. Here's how to convert:

### 1. Install Npgsql Package

Add to `BloodNET-Web.csproj`:
```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.4" />
```

### 2. Update Program.cs

Replace SQL Server configuration:
```csharp
// OLD:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));

// NEW:
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
```

### 3. Update Repository Classes

The repository classes use direct SQL queries with SQL Server syntax. These need to be updated:

**BloodDonorsRepository.cs**, **BloodRequestsRepository.cs**, etc.:
- Replace `NVARCHAR` with `VARCHAR` or `TEXT`
- Replace `IDENTITY(1,1)` with `SERIAL` or `GENERATED ALWAYS AS IDENTITY`
- Replace parameter syntax from `@param` to `$1, $2, etc.` if needed (Dapper handles this automatically)

### 4. Update SQL Scripts

The scripts in `Tables-Scripts/` folder should be updated for PostgreSQL:

**Example for BloodDonors.sql**:
```sql
CREATE TABLE BloodDonors (
    DonorId VARCHAR(50) PRIMARY KEY,
    Name VARCHAR(100) NOT NULL,
    DOB DATE NOT NULL,
    Gender VARCHAR(10),
    Weight DOUBLE PRECISION,
    WeightUnit VARCHAR(20),
    BloodGroup VARCHAR(10),
    PhoneNumber VARCHAR(20),
    Email VARCHAR(100),
    Address VARCHAR(255),
    City VARCHAR(100),
    Country VARCHAR(100),
    MedicalHistory TEXT,
    DonorStatus INTEGER,
    ImgUrl VARCHAR(255)
);
```

### 5. Create New Migration for PostgreSQL

```bash
# Remove old migrations (backup first!)
# Or create new migration
dotnet ef migrations add InitialPostgreSQL
dotnet ef database update
```

---

## Docker Deployment (Works with Any Platform)

Create `Dockerfile` in the root directory:

```dockerfile
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src
COPY ["BloodNET-Web/BloodNET-Web.csproj", "BloodNET-Web/"]
RUN dotnet restore "BloodNET-Web/BloodNET-Web.csproj"
COPY . .
WORKDIR "/src/BloodNET-Web"
RUN dotnet build "BloodNET-Web.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "BloodNET-Web.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "BloodNET-Web.dll"]
```

Create `.dockerignore`:
```
**/.git
**/.vs
**/.vscode
**/bin
**/obj
**/.DS_Store
**/node_modules
```

Build and run locally:
```bash
docker build -t bloodnet .
docker run -p 8080:80 bloodnet
```

---

## Environment Configuration

### Production appsettings

Create `appsettings.Production.json`:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "YOUR_PRODUCTION_CONNECTION_STRING"
  },
  "Logging": {
    "LogLevel": {
      "Default": "Information",
      "Microsoft.AspNetCore": "Warning"
    }
  },
  "AllowedHosts": "*"
}
```

### Environment Variables

Most platforms support environment variables. Use double underscore (`__`) to represent nested configuration:

```bash
ConnectionStrings__DefaultConnection=Server=...;Database=...
ASPNETCORE_ENVIRONMENT=Production
```

---

## Database Migration Strategy

### For New Deployment:

1. **Ensure migrations are created**:
   ```bash
   dotnet ef migrations add InitialCreate
   ```

2. **Apply migrations on startup** (already configured in `Program.cs` for development)

3. **Or apply manually**:
   ```bash
   dotnet ef database update
   ```

### For Existing Database:

1. Run the SQL scripts in `Tables-Scripts/` directory manually on your production database
2. Or use EF Core migrations

---

## Recommended Approach: Railway with PostgreSQL

For the easiest free deployment, I recommend **Railway**:

1. **Pros**:
   - Simplest setup
   - Automatic deployments from GitHub
   - Includes PostgreSQL database
   - $5 free credit per month (usually enough for small apps)
   - Automatic HTTPS
   - No sleep/cold starts

2. **Setup Steps**:
   - Update code to use PostgreSQL (I can help with this)
   - Push to GitHub
   - Connect Railway to GitHub
   - Add PostgreSQL database in Railway
   - Deploy automatically

3. **Estimated Monthly Cost**: Free (within $5 credit)

---

## Alternative: Azure with Supabase

If you prefer to stick with Azure App Service but need a free database:

1. **Use Azure App Service Free Tier** for hosting
2. **Use Supabase** (free PostgreSQL database - 500MB)
   - Sign up at [supabase.com](https://supabase.com)
   - Create new project
   - Get connection string
   - Use in Azure App Service configuration

---

## Security Considerations for Production

1. **Connection Strings**: Never commit production connection strings to Git
2. **User Secrets**: Use Azure Key Vault, Railway environment variables, or similar
3. **HTTPS**: Ensure HTTPS is enforced (automatic on Railway, Render, Azure)
4. **Email Confirmation**: The app requires email confirmation (`options.SignIn.RequireConfirmedAccount = true`)
   - Configure an email service (SendGrid has free tier)
   - Or disable in production: `options.SignIn.RequireConfirmedAccount = false`

---

## Next Steps

1. **Choose a platform** based on your preferences
2. **Decide on database**: PostgreSQL (easier for free hosting) or SQL Server (Azure)
3. **Update configuration** files
4. **Set up CI/CD** (optional but recommended)
5. **Configure monitoring** (most platforms include basic monitoring)

---

## Support and Resources

- **Azure**: https://docs.microsoft.com/azure/app-service/
- **Railway**: https://docs.railway.app/
- **Render**: https://render.com/docs
- **Fly.io**: https://fly.io/docs/
- **Entity Framework Core**: https://docs.microsoft.com/ef/core/

---

## Questions?

If you need help with any specific deployment option, please let me know which platform you prefer, and I can provide detailed step-by-step instructions and make the necessary code changes.
