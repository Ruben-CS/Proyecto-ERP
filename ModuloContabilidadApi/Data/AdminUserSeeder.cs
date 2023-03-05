using Microsoft.AspNetCore.Identity;
using ModuloContabilidadApi.Models;

namespace ModuloContabilidadApi.Data;

public static class AdminUserSeeder
{
    public static async Task SeedAdminUserAsync(UserManager<Usuario>
                                                    userManager)
    {
        var adminUser = await userManager.FindByNameAsync("admin") ??
                        new Usuario()
                        {
                            Nombre   = "admin",
                            UserName = "admin",
                        };

        var password     = "Passw0rd";
        var passwordHash = new PasswordHasher<Usuario>();

        adminUser.PasswordHash = passwordHash.HashPassword(adminUser, password);
        await userManager.CreateAsync(adminUser);
    }
}