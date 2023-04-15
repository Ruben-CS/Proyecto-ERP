using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos.ApplicationContexts;

namespace Services.Cuenta;

internal static class CuentaUtility
{
    public static async Task<string> GenerarCodigo(_applicationDbContext dbContext,
                                                   int? idCuentaPadre, int idEmpresa,
                                                   int numeroNiveles, int nivelActual = 0)
    {
        try
        {
            if (idCuentaPadre is null or 0)

            {
                //null here
                var cuentasPadre = await dbContext.Cuentas.Where(c =>
                    c.IdCuentaPadre == null &&
                    c.IdEmpresa     == idEmpresa).ToListAsync();

                if (cuentasPadre.IsNullOrEmpty())

                {
                    Console.WriteLine("here");
                }

                var siguienteNumeroPadre = cuentasPadre.Count > 0
                    ? cuentasPadre.Max(c => int.Parse(c.Codigo.Split('.')[0])) + 1
                    : 1;

                return
                    $"{siguienteNumeroPadre}" +
                    $"{string.Concat(Enumerable.Repeat(".0", numeroNiveles - 1))}";
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        var padre       = await dbContext.Cuentas.FindAsync(idCuentaPadre);
        var codigoPadre = padre.Codigo.Split('.');
        var hijos = await dbContext.Cuentas
                                   .Where(c =>
                                       c.IdCuentaPadre == idCuentaPadre)
                                   .ToListAsync();
        if (codigoPadre[nivelActual] is not "0")
            return await await Task.FromResult(GenerarCodigo(dbContext,idCuentaPadre, idEmpresa,
                numeroNiveles,
                nivelActual + 1));
        {
            var siguienteNumeroHijo = hijos.Count > 0
                ? hijos.Max(c => int.Parse(c.Codigo.Split('.')[nivelActual])) + 1
                : 1;
            codigoPadre[nivelActual] = $"{siguienteNumeroHijo}";
            return await Task.FromResult(string.Join(".", codigoPadre));
        }
    }
}