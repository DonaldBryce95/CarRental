using System.Text;
using CarRental.Api.Exceptions;
using CarRental.Application.Interfaces;
using CarRental.Application.Mappings;
using CarRental.Application.Services;
using CarRental.Infrastructure.Data;
using CarRental.Infrastructure.Identity;
using CarRental.Infrastructure.Repositories;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
// Learn more about configuring OpenAPI at https://aka.ms/aspnet/openapi
builder.Services.AddOpenApi();
builder.Services.AddControllers();
builder.Services.AddDbContext<CarRentalDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));
builder.Services.AddExceptionHandler<GlobalExceptionHandler>();
builder.Services.AddProblemDetails();
builder.Services.AddIdentity<ApplicationUser, IdentityRole>().AddEntityFrameworkStores<CarRentalDbContext>().AddDefaultTokenProviders();
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,

        ValidIssuer = builder.Configuration["Jwt:Issuer"],
        ValidAudience = builder.Configuration["Jwt:Audience"],

        IssuerSigningKey = new SymmetricSecurityKey(
            Encoding.UTF8.GetBytes(builder.Configuration["Jwt:Key"]!))
    };
});
builder.Services.AddScoped<IAdminRepository, AdminRepository>();
builder.Services.AddScoped<IAdminService, AdminService>();
builder.Services.AddScoped<ICustomerRepository, CustomerRepository>();
builder.Services.AddScoped<ICustomerService, CustomerService>();
builder.Services.AddScoped<IMaintenanceService, MaintenanceService>();
builder.Services.AddScoped<IMaintenanceRepository, MaintenanceRepository>();
builder.Services.AddAutoMapper(cfg =>
{
}, typeof(BranchProfile).Assembly);

builder.Services.AddCors(options =>
{
    options.AddPolicy("ReactClient", policy =>
    {
        policy
            .WithOrigins(
                "http://localhost:5173",
                "http://localhost:8081"
            )
            .AllowAnyHeader()
            .AllowAnyMethod();
    });
});

var app = builder.Build();

using (var scope = app.Services.CreateScope())
{
    var roleManager = scope.ServiceProvider.GetRequiredService<RoleManager<IdentityRole>>();
    var userManager = scope.ServiceProvider.GetRequiredService<UserManager<ApplicationUser>>();

    string[] roles = { "Admin", "Customer", "MaintenanceWorker" };

    foreach (var role in roles)
    {
        if (!await roleManager.RoleExistsAsync(role))
        {
            await roleManager.CreateAsync(new IdentityRole(role));
        }
    }

    var adminEmail = builder.Configuration["SeedAdmin:Email"];
    var adminPassword = builder.Configuration["SeedAdmin:Password"];

    if (string.IsNullOrWhiteSpace(adminEmail) ||
        string.IsNullOrWhiteSpace(adminPassword))
    {
        throw new InvalidOperationException(
        "Seed admin credentials are not configured.");
    }

    var admin = await userManager.FindByEmailAsync(adminEmail);

    if (admin == null)
    {
        admin = new ApplicationUser
        {
            UserName = adminEmail,
            Email = adminEmail,
            EmailConfirmed = true
        };

        var result = await userManager.CreateAsync(admin, adminPassword);

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to create admin: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }

    if (!await userManager.IsInRoleAsync(admin, "Admin"))
    {
        var result = await userManager.AddToRoleAsync(admin, "Admin");

        if (!result.Succeeded)
        {
            throw new Exception($"Failed to assign Admin role: {string.Join(", ", result.Errors.Select(e => e.Description))}");
        }
    }
}

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.MapOpenApi();
}

app.UseExceptionHandler();
app.UseHttpsRedirection();
app.UseCors("ReactClient");

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();
