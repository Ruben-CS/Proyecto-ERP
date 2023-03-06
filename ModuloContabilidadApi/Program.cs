using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Data;
using ModuloContabilidadApi.Data.Interfaces;
using ModuloContabilidadApi.Models;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configuracion del usuario
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

builder.Services.AddScoped<IDataSeeder, AdminUserSeeder>();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"));
});

builder.Services.Configure<IdentityOptions>(options =>
{
    options.Password.RequireDigit           = false;
    options.Password.RequireLowercase       = false;
    options.Password.RequireNonAlphanumeric = false;
    options.Password.RequireUppercase       = false;
    options.Password.RequiredLength         = 0;
});


var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

using (var scope = app.Services.CreateScope())
{
    try
    {
        var dataSeeder =
            scope.ServiceProvider.GetRequiredService<IDataSeeder>();

        dataSeeder.SeedAsync().Wait();
    }
    catch (Exception ex)
    {
        var logger = app.Services.GetRequiredService<ILogger<Program>>();
        logger.LogError(ex, "An error occurred while seeding the database.");
    }
}

app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.Run();