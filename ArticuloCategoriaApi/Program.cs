using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Services.MapConfiguration;
using Services.Repository;
using Services.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var mapper  = MappingConfiguration.RegisterMaps().CreateMapper();


// Add services to the container.

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


builder.Services.AddSingleton(mapper);


builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Modelos"));
});


builder.Services.AddScoped<ICategoriaRepository, CategoriaRepository>();
builder.Services.AddScoped<IArticuloRepository, ArticuloRepository>();
builder.Services.AddScoped<IArticuloCategoriaRepository,ArticuloCategoriaRepository>();
builder.Services.AddScoped<INotaRepository, NotaRepository>();
builder.Services.AddScoped<ILoteRepository, LoteRepository>();
builder.Services.AddMemoryCache();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();