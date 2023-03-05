using Microsoft.AspNetCore.Identity;
using ModuloContabilidadApi.Data.Interfaces;
using ModuloContabilidadApi.Models;

namespace ModuloContabilidadApi.Data;

public class DataSeeder
{
    private readonly IServiceProvider _serviceProvider;

    public DataSeeder(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }

    public async Task SeedAsync()
    {
        using var scope = _serviceProvider.CreateScope();
        var userManager = scope.ServiceProvider
            .GetRequiredService<UserManager<Usuario>>();

        var adminUserSeeder = new AdminUserSeeder(userManager);
        await adminUserSeeder.SeedAsync();
    }
}