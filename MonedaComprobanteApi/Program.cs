using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Newtonsoft.Json.Serialization;
using Services.MapConfiguration;
using Services.Repository;
using Services.Repository.Interfaces;

var builder = WebApplication.CreateBuilder(args);
var mapper  = MappingConfiguration.RegisterMaps().CreateMapper();

builder.Services.AddControllers().AddNewtonsoftJson(options => options.SerializerSettings.ContractResolver = new CamelCasePropertyNamesContractResolver());
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

builder.Services.AddDbContext<ApplicationDbContext>(o =>
{
    o.UseSqlServer(
        builder.Configuration.GetConnectionString("DefaultConnection"),
        b => b.MigrationsAssembly("Modelos"));
});

builder.Services.AddSingleton(mapper);
builder.Services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());

builder.Services.AddScoped<IMonedaRepository, MonedaRepository>();
builder.Services.AddScoped<IEmpresaMonedaRepository, EmpresaMonedaRepository>();
builder.Services.AddScoped<IComprobanteRepository, ComprobanteRepository>();
builder.Services.AddScoped<IDetalleComprobanteRepository, DetalleComprobanteRepository>();
builder.Services.AddSwaggerGenNewtonsoftSupport();


var app = builder.Build();

if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI();
}

app.UseHttpsRedirection();

app.UseAuthorization();

app.MapControllers();

app.Run();