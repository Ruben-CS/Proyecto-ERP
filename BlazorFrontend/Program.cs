using MudBlazor.Services;
using BlazorFrontend.Services;
using Services.Cuenta;
using Services.EmpresaMonedaService;
using Services.Gestion;
using Services.Moneda;
using Services.Periodo;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<GestionServices>();
builder.Services.AddScoped<PeriodoService>();
builder.Services.AddScoped<CuentaService>();
builder.Services.AddScoped<MonedaService>();
builder.Services.AddScoped<EmpresaMonedaService>();
var app = builder.Build();


if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    app.UseHsts();
}
app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();
app.UseAuthorization();
app.UseAuthentication();


app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();