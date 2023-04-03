using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using ModuloContabilidadApi;
using ModuloContabilidadApi.Repository;
using ModuloContabilidadApi.Repository.Interfaces;
using Services.Gestion;
using Services.Repository;
using Services.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var mapper  = MappingConfiguration.RegisterMaps().CreateMapper();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

//configuracion del usuario


builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"), b => b.MigrationsAssembly("Modelos"));
});


builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
builder.Services.AddScoped<IEmpresaRepository, EmpresaRepository>();
builder.Services.AddScoped<IGestionRepository, GestionRepository>();
builder.Services.AddScoped<IAuthRepository, AuthRepository>();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}


app.UseHttpsRedirection();

app.UseAuthentication();
app.UseAuthorization();

app.MapControllers();
app.Run();