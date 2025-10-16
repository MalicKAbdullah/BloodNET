# PostgreSQL Conversion Guide

## Overview

This guide helps you convert BloodNET from SQL Server to PostgreSQL for free hosting platforms like Railway, Render, or Fly.io.

## Why PostgreSQL?

Most free hosting platforms offer PostgreSQL because:
- It's open source (no licensing costs)
- Lower resource requirements
- Better for containerized deployments
- More common in cloud-native environments

## Step-by-Step Conversion

### Step 1: Add PostgreSQL Package

Add Npgsql package to `BloodNET-Web.csproj`:

```xml
<PackageReference Include="Npgsql.EntityFrameworkCore.PostgreSQL" Version="8.0.4" />
```

Then restore packages:
```bash
dotnet restore
```

### Step 2: Update Program.cs

**Find this line:**
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
```

**Replace with:**
```csharp
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseNpgsql(connectionString));
```

**Add using statement at the top:**
```csharp
using Npgsql.EntityFrameworkCore.PostgreSQL;
```

### Step 3: Update Connection String Format

PostgreSQL uses a different connection string format.

**SQL Server format:**
```
Server=localhost;Database=BloodNET;User Id=sa;Password=xxx;TrustServerCertificate=True;
```

**PostgreSQL format:**
```
Host=localhost;Database=BloodNET;Username=postgres;Password=xxx;Port=5432;SSL Mode=Prefer;Trust Server Certificate=true;
```

**For Railway (environment variables):**
```
Host=${{Postgres.PGHOST}};Database=${{Postgres.PGDATABASE}};Username=${{Postgres.PGUSER}};Password=${{Postgres.PGPASSWORD}};Port=${{Postgres.PGPORT}};SSL Mode=Require;Trust Server Certificate=true;
```

### Step 4: Create New Migrations for PostgreSQL

Remove or backup existing migrations:
```bash
cd BloodNET-Web

# Optional: backup old migrations
mkdir ../migrations-backup
cp -r Data/Migrations/* ../migrations-backup/

# Remove old migrations (or keep and add new)
rm -rf Data/Migrations/*
```

Create new PostgreSQL migrations:
```bash
dotnet ef migrations add InitialCreate --context ApplicationDbContext
```

### Step 5: Update Repository Classes

The repositories use raw SQL with SQL Server-specific syntax. Update these files:

#### BloodDonorsRepository.cs
#### BloodRequestsRepository.cs  
#### ContactsRepository.cs
#### DonationRepository.cs
#### AdminRepository.cs

**Changes needed:**
1. Parameter placeholders work the same with Dapper (`@param`)
2. `IDENTITY(1,1)` → Use `SERIAL` or let PostgreSQL auto-generate
3. `NVARCHAR` → `VARCHAR` or `TEXT` (handled by Dapper automatically)
4. Date functions: `GETDATE()` → `NOW()`

**Example fix in BloodDonorsRepository.cs:**

Replace:
```csharp
string insertQuery = "INSERT INTO BloodDonors(...) VALUES(...)";
```

With (no changes needed for parameters, Dapper handles it):
```csharp
// Same query works with PostgreSQL via Dapper
string insertQuery = "INSERT INTO BloodDonors(...) VALUES(...)";
```

### Step 6: Update Database Scripts (Optional)

If you're using the scripts in `Tables-Scripts/`:

**SQL Server:**
```sql
CREATE TABLE BloodDonors (
    DonorId NVARCHAR(50) PRIMARY KEY,
    Name NVARCHAR(100) NOT NULL,
    ...
);
```

**PostgreSQL:**
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

### Step 7: Test Locally with PostgreSQL

Using Docker:
```bash
# Start PostgreSQL
docker run --name bloodnet-postgres -e POSTGRES_PASSWORD=yourpassword -e POSTGRES_DB=BloodNET -p 5432:5432 -d postgres:15

# Update appsettings.json with PostgreSQL connection
# Run migrations
dotnet ef database update

# Run application
dotnet run
```

Using Railway:
```bash
# Deploy to Railway (it will use Railway's PostgreSQL)
git push
```

### Step 8: Run Migrations on Production

**Option A: Automatic (Recommended)**

Add to `Program.cs` before `app.Run()`:
```csharp
// Apply pending migrations automatically on startup
using (var scope = app.Services.CreateScope())
{
    var services = scope.ServiceProvider;
    try
    {
        var context = services.GetRequiredService<ApplicationDbContext>();
        context.Database.Migrate();
        
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogInformation("Database migration completed successfully.");
    }
    catch (Exception ex)
    {
        var logger = services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while migrating the database.");
        throw; // Re-throw to prevent app from starting with failed migration
    }
}
```

**Option B: Manual**

Run migrations manually with connection string:
```bash
dotnet ef database update --connection "Host=xxx;Database=xxx;Username=xxx;Password=xxx;Port=5432"
```

---

## Common Issues and Solutions

### Issue 1: Case Sensitivity

PostgreSQL is case-sensitive for quoted identifiers but lowercases unquoted identifiers.

**Solution:** EF Core handles this automatically. If using raw SQL, be consistent with casing.

### Issue 2: Data Type Differences

| SQL Server | PostgreSQL |
|------------|-----------|
| NVARCHAR | VARCHAR or TEXT |
| BIT | BOOLEAN |
| DATETIME | TIMESTAMP |
| INT IDENTITY | SERIAL or INTEGER with SEQUENCE |

**Solution:** EF Core migrations handle this automatically.

### Issue 3: String Comparison

SQL Server is case-insensitive by default; PostgreSQL is case-sensitive.

**Solution:** Use `ILIKE` for case-insensitive searches in raw SQL:
```sql
-- SQL Server
WHERE Name LIKE '%john%'

-- PostgreSQL
WHERE Name ILIKE '%john%'
```

Or use `EF.Functions.ILike()` in LINQ.

### Issue 4: True/False vs 1/0

SQL Server uses `BIT` (0/1); PostgreSQL uses `BOOLEAN` (true/false).

**Solution:** EF Core handles conversion automatically. In raw SQL, use `TRUE`/`FALSE` instead of `1`/`0`.

---

## Testing PostgreSQL Compatibility

1. **Install PostgreSQL locally** or use Docker:
   ```bash
   docker run -d --name postgres -p 5432:5432 -e POSTGRES_PASSWORD=test postgres:15
   ```

2. **Update connection string** in `appsettings.Development.json`:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Host=localhost;Database=BloodNET;Username=postgres;Password=test;Port=5432"
   }
   ```

3. **Run migrations**:
   ```bash
   dotnet ef database update
   ```

4. **Run application**:
   ```bash
   dotnet run
   ```

5. **Test all features**: Create donors, requests, donations, etc.

---

## Benefits of PostgreSQL

1. **Free hosting** - Available on Railway, Render, Fly.io, Heroku
2. **Better performance** - For many workloads
3. **Advanced features** - JSON support, full-text search, arrays
4. **No licensing costs** - Open source
5. **Great documentation** - Extensive community support

---

## Rollback to SQL Server

If you need to rollback:

1. **Revert Program.cs**:
   ```csharp
   options.UseSqlServer(connectionString)
   ```

2. **Update connection string** to SQL Server format

3. **Restore old migrations** from backup

4. **Run migrations**:
   ```bash
   dotnet ef database update
   ```

---

## Next Steps

1. ✅ Add Npgsql package
2. ✅ Update Program.cs  
3. ✅ Update connection string
4. ✅ Create new migrations
5. ✅ Test locally (optional)
6. ✅ Deploy to Railway/Render
7. ✅ Run production migrations

---

## Need Help?

If you encounter issues during conversion:
1. Check the error message
2. Verify connection string format
3. Ensure PostgreSQL is running
4. Check that migrations are applied
5. Review Railway/Render logs

Let me know if you need assistance with any step!
