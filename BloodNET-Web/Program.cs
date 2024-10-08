using BloodNET_Web.Controllers;
using BloodNET_Web.Data;
using BloodNET_Web.Models;
using BloodNET_Web.Models.Hubs;
using BloodNET_Web.Models.Interfaces;
using BloodNET_Web.Models.Repository;
using Microsoft.AspNet.SignalR;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.SignalR;
using Microsoft.EntityFrameworkCore;
using System.Configuration;
using System.Security.Claims;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
builder.Services.AddDbContext<ApplicationDbContext>(options =>
    options.UseSqlServer(connectionString));
builder.Services.AddDatabaseDeveloperPageExceptionFilter();

builder.Services.AddDefaultIdentity<MyUsers>(options => options.SignIn.RequireConfirmedAccount = true)
    .AddEntityFrameworkStores<ApplicationDbContext>();

builder.Services.AddAuthorization(options =>
{
    options.AddPolicy("AdminPolicy", policy =>
        policy.RequireClaim(ClaimTypes.Email, "admin@bloodnet.com"));
});

builder.Services.AddSignalR();

builder.Services.AddScoped<IBloodDonors, BloodDonorsRepository>();
builder.Services.AddScoped<IAdmin, AdminRepository>();
builder.Services.AddScoped<IBloodRequests, BloodRequestsRepository>();
builder.Services.AddScoped<IContacts, ContactsRepository>();
builder.Services.AddScoped<IDonation, DonationRepository>();

builder.Services.AddScoped(typeof(IRepository<>), typeof(GenericRepository<>));



// Register DonorController for dependency injection
builder.Services.AddTransient<DonorController>();

builder.Services.AddDistributedMemoryCache();
builder.Services.AddSession();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseMigrationsEndPoint();
}
else
{
    app.UseExceptionHandler("/Home/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseSession();

app.UseAuthorization();

app.MapControllerRoute(
    name: "default",
    pattern: "{controller=Home}/{action=Index}/{id?}");
app.MapRazorPages();

app.MapHub<BloodRequestHub>("/bloodRequestsHub");

app.MapHub<BloodDonationHub>("/bloodDonationHub");

app.Run();
