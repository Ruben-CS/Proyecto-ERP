using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository;
using ModuloContabilidadApi.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var mapper  = MappingConfiguration.RegisterMaps().CreateMapper();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configuracion del usuario
builder.Services.AddIdentity<Usuario, IdentityRole>()
    .AddEntityFrameworkStores<ApplicationDbContext>()
    .AddDefaultTokenProviders();

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

builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthorization();
app.UseAuthentication();

app.MapControllers();
app.Run();