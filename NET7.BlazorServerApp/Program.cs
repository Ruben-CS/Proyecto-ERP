using NET7.BlazorServerApp.Data;
using BlazorBootstrap;
using System.Net.Http.Json;
using MudBlazor.Services;
using NET7.BlazorServerApp.Services;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddBlazorBootstrap();
builder.Services.AddScoped<HttpClient>();
builder.Services.AddMudServices();
builder.Services.AddHttpClient();
builder.Services.AddScoped<EmpresaService>();

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