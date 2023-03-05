using Microsoft.AspNetCore.Identity;
using ModuloContabilidadApi.Data.Interfaces;
using ModuloContabilidadApi.Models;

namespace ModuloContabilidadApi.Data;

public class AdminUserSeeder : IDataSeeder
{
    private readonly UserManager<Usuario> _userManager;

    public AdminUserSeeder(UserManager<Usuario> userManager)
    {
        _userManager = userManager;
    }

    public async Task SeedAsync()
    {
        var adminUser = await _userManager.FindByNameAsync("admin") ??
                        new Usuario()
                        {
                            Nombre   = "admin",
                            UserName = "admin",
                        };

        var password     = "Passw0rd";
        var passwordHash = new PasswordHasher<Usuario>();

        adminUser.PasswordHash = passwordHash.HashPassword(adminUser, password);

        var result = await _userManager.CreateAsync(adminUser);

        if (result.Succeeded)
        {
            Console.WriteLine("User created succesfully");
            await _userManager.AddToRoleAsync(adminUser, "Admin");
        }
        else
        {
            var errorMessage = string.Join(",", result.Errors.Select(error =>
                error.Description));
            Console.WriteLine($"Admin user creation failed : {errorMessage}");
        }
    }
}