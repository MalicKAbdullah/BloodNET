# Railway Deployment Guide for BloodNET

## Quick Start

Railway is the easiest way to deploy BloodNET for free with a PostgreSQL database included.

### Step 1: Prepare Your Repository

1. Ensure all code changes are committed to your GitHub repository
2. The following files are already configured:
   - `railway.json` - Railway configuration
   - `Dockerfile` - Container configuration
   - `DEPLOYMENT.md` - Full deployment documentation

### Step 2: Sign Up for Railway

1. Go to [Railway.app](https://railway.app)
2. Sign up using your GitHub account (recommended for easy deployment)

### Step 3: Create New Project

1. Click **"New Project"**
2. Select **"Deploy from GitHub repo"**
3. Choose your `BloodNET` repository
4. Railway will automatically detect the .NET application

### Step 4: Add PostgreSQL Database

1. In your Railway project, click **"New"** → **"Database"** → **"Add PostgreSQL"**
2. Railway will create a PostgreSQL database and provide connection details
3. Note the connection string (Railway provides this automatically as an environment variable)

### Step 5: Configure Environment Variables

Add these environment variables in Railway dashboard:

1. **ConnectionStrings__DefaultConnection** (Railway will auto-populate with PostgreSQL connection):
   ```
   Host=${{Postgres.PGHOST}};Database=${{Postgres.PGDATABASE}};Username=${{Postgres.PGUSER}};Password=${{Postgres.PGPASSWORD}};Port=${{Postgres.PGPORT}};SSL Mode=Require;Trust Server Certificate=true;
   ```

2. **ASPNETCORE_ENVIRONMENT**:
   ```
   Production
   ```

3. **ASPNETCORE_URLS**:
   ```
   http://0.0.0.0:8080
   ```

### Step 6: Update Code for PostgreSQL

Before deploying, you need to convert from SQL Server to PostgreSQL. I can help with this - just let me know!

#### Quick Changes Needed:

1. **Install Npgsql package** (already done below)
2. **Update Program.cs** to use PostgreSQL
3. **Update repository classes** for PostgreSQL compatibility
4. **Create new migrations** for PostgreSQL

### Step 7: Deploy

1. Push changes to GitHub
2. Railway will automatically:
   - Detect changes
   - Build the application
   - Deploy to production
   - Provide a URL (e.g., `yourapp.railway.app`)

### Step 8: Run Database Migrations

After first deployment:
1. In Railway dashboard, go to your service
2. Click on **"Settings"** → **"Deploy"**
3. You may need to run migrations manually once:

```bash
# Connect to Railway shell (if available) or run locally pointing to Railway DB
dotnet ef database update --connection "YOUR_RAILWAY_CONNECTION_STRING"
```

Or enable automatic migrations on startup (see below).

---

## Automatic Database Migration on Startup

Add this to `Program.cs` before `app.Run()`:

```csharp
// Apply pending migrations on startup
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
```

---

## Cost Estimate

Railway provides:
- **$5 free credit per month**
- Typical usage for a small app: $3-5/month
- PostgreSQL database included in usage

**The free tier should cover:**
- 500 hours of runtime/month
- PostgreSQL database with reasonable usage
- Basic traffic levels

---

## PostgreSQL Connection String Format

Railway provides these variables automatically:
- `PGHOST` - Database host
- `PGPORT` - Database port (usually 5432)
- `PGUSER` - Database username
- `PGPASSWORD` - Database password
- `PGDATABASE` - Database name

Railway auto-injects these, so you can reference them as:
```
${{Postgres.PGHOST}}
```

---

## Troubleshooting

### Build Fails
- Check that all NuGet packages are restored
- Ensure .NET 8.0 SDK is specified in configuration
- Check build logs in Railway dashboard

### Application Won't Start
- Verify `ASPNETCORE_URLS` is set to `http://0.0.0.0:8080`
- Check that port 8080 is exposed in Dockerfile
- Review application logs in Railway dashboard

### Database Connection Issues
- Verify connection string format for PostgreSQL
- Check that SSL Mode is set to "Require"
- Ensure database service is running in Railway

### Migrations Not Applied
- Run migrations manually first deployment
- Or enable automatic migrations on startup
- Check that connection string has write permissions

---

## Monitoring and Logs

Railway provides:
- **Real-time logs** in the dashboard
- **Metrics** for CPU, memory, and network usage
- **Deployment history** and rollback capability

---

## Scaling

On Railway's paid plans ($5+ per month):
- Vertical scaling (more CPU/RAM)
- Horizontal scaling (multiple instances)
- Custom domains
- Increased database storage

---

## Next Steps

1. **Convert to PostgreSQL** - Let me know if you need help with this
2. **Push to GitHub**
3. **Deploy to Railway**
4. **Configure custom domain** (optional)
5. **Set up monitoring** (Railway provides this)

---

## Alternative: Keep SQL Server with Azure

If you prefer to keep SQL Server instead of PostgreSQL:
1. Use Azure App Service (Free Tier)
2. Use Azure SQL Database (Basic Tier - $5/month) or Supabase
3. See `DEPLOYMENT.md` for detailed Azure instructions

---

## Questions?

Let me know if you:
- Need help converting to PostgreSQL
- Want detailed PostgreSQL migration instructions
- Prefer a different deployment platform
- Have any deployment issues
