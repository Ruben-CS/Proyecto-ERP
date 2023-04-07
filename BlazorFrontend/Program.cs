using MudBlazor.Services;
using BlazorFrontend.Services;
using Services.Gestion;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmpresaService>();
builder.Services.AddScoped<GestionServices>();
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