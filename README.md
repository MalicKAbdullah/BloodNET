# BloodNET-Web

## Overview
BloodNET is a blood donation management system built with ASP.NET Core 8.0. It helps connect blood donors with recipients who need blood donations.

## Features
- User registration and authentication
- Blood donor management
- Blood request management
- Real-time notifications using SignalR
- Admin dashboard
- Contact form

## Technology Stack
- **Framework**: ASP.NET Core 8.0
- **Database**: SQL Server (with PostgreSQL support)
- **ORM**: Entity Framework Core + Dapper
- **Authentication**: ASP.NET Identity
- **Real-time**: SignalR
- **Frontend**: Razor Pages, Bootstrap, jQuery

## Free Deployment Options

This application can be deployed for **FREE** on several platforms:

### 🚀 Recommended: Railway (Easiest)
- **Cost**: $5 free credit/month
- **Includes**: PostgreSQL database
- **Setup Time**: 10 minutes
- **Guide**: See [RAILWAY.md](RAILWAY.md)

### ☁️ Azure App Service
- **Cost**: Free tier available
- **Includes**: Web hosting (database separate)
- **Best for**: .NET applications
- **Guide**: See [AZURE.md](AZURE.md)

### 🐳 Docker + Any Platform
- **Cost**: Varies by platform
- **Portability**: Works anywhere
- **Guide**: See [DEPLOYMENT.md](DEPLOYMENT.md)

## Quick Start Guides

1. **[DEPLOYMENT.md](DEPLOYMENT.md)** - Complete deployment guide with all options
2. **[RAILWAY.md](RAILWAY.md)** - Quick Railway deployment (recommended)
3. **[AZURE.md](AZURE.md)** - Azure App Service deployment
4. **[POSTGRESQL.md](POSTGRESQL.md)** - Converting from SQL Server to PostgreSQL

## Local Development

### Prerequisites
- .NET 8.0 SDK
- SQL Server or PostgreSQL
- Visual Studio 2022 or VS Code

### Setup
1. Clone the repository
   ```bash
   git clone https://github.com/MalicKAbdullah/BloodNET.git
   cd BloodNET
   ```

2. Update connection string in `appsettings.json`
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=BloodNET;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. Run migrations
   ```bash
   cd BloodNET-Web
   dotnet ef database update
   ```

4. Run the application
   ```bash
   dotnet run
   ```

5. Open browser to `https://localhost:5001`

## Database

The application uses the following tables:
- **AspNetUsers** - User accounts (Identity)
- **BloodDonors** - Donor information
- **BloodRequests** - Blood requests
- **Donation** - Donation records
- **Contacts** - Contact form submissions
- **Respondants** - Response tracking

SQL scripts are available in the `Tables-Scripts` directory.

## Docker Support

Build and run with Docker:
```bash
docker build -t bloodnet .
docker run -p 8080:8080 bloodnet
```

Or use Docker Compose (includes SQL Server):
```bash
docker-compose up
```

## CI/CD

GitHub Actions workflows are configured for:
- **Build and Test** - Runs on every push
- **Azure Deployment** - Manual deployment to Azure

## Contributing

Contributions are welcome! Please feel free to submit a Pull Request.

## License

This project is open source and available under the MIT License.

## Support

For deployment help or questions:
1. Check the deployment guides in the repository
2. Open an issue on GitHub
3. Review the documentation links in each guide

## Deployment Decision Tree

**Choose your deployment platform:**

- **Want easiest setup?** → Use [Railway](RAILWAY.md)
- **Want to stick with SQL Server?** → Use [Azure](AZURE.md)
- **Want maximum control?** → Use [Docker](DEPLOYMENT.md)
- **Have technical experience?** → Any option works!

All options have **FREE tiers** available! 🎉