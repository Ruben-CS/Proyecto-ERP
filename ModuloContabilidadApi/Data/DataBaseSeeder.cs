using Microsoft.AspNetCore.Identity;
using ModuloContabilidadApi.Models;

namespace ModuloContabilidadApi.Data;

public class DataBaseSeeder : IHostedService
{
    private readonly UserManager<Usuario> _userManager;

    public DataBaseSeeder(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }
    public async Task StartAsync(CancellationToken cancellationToken)
    {
        var user = await _userManager.FindByNameAsync("admin");
        if (user == null)
        {
            user = new Usuario()
            {
                Nombre   = "admin",
                Password = "admin"
            };
            await _userManager.CreateAsync(user, "admin");
        }
    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        return Task.CompletedTask;
    }
}