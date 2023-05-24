using Blazored.LocalStorage;
using MudBlazor.Services;
using BlazorFrontend.Services;
using BlazorFrontend.Utility_Class;
using Services.ArticuloService;
using Services.CategoriaService;
using Services.Comprobante;
using Services.Cuenta;
using Services.DetalleComprobante;
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
builder.Services.AddScoped<ComprobanteService>();
builder.Services.AddScoped<AppStateService>();
builder.Services.AddScoped<DetalleComprobanteService>();
builder.Services.AddScoped<CategoriaService>();
builder.Services.AddScoped<ArticuloService>();
builder.Services.AddBlazoredLocalStorage();
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