using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;

namespace Services.Validators;

public static class CuentaValidations
{
    public static async Task<bool> ValidateUniqueNameAsync(
        ApplicationDbContext dbContext, CuentaDto cuentaDto)
    {
        return await await Task.FromResult(
            dbContext.Cuentas.AnyAsync(c =>
                string.Equals(c.Nombre, cuentaDto.Nombre,
                    StringComparison.OrdinalIgnoreCase)));
    }

    public static async Task<bool> ExisteParentAccountAsync(
        ApplicationDbContext dbContext, CuentaDto cuentaDto)
    {
        if (cuentaDto.IdCuentaPadre is null or 0)
            return await Task.FromResult(false);
        var cuentaPadre =
            await dbContext.Cuentas.FindAsync(cuentaDto.IdCuentaPadre.Value);
        if (cuentaPadre is not null)
            return await Task.FromResult(true);

        return await Task.FromResult(false);
    }
}