using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;

namespace Services.Cuenta;

internal static class CuentaUtility
{
    public static async Task<string> GenerarCodigo(ApplicationDbContext dbContext,
                                                   int? idCuentaPadre, int idEmpresa,
                                                   int numeroNiveles, int nivelActual = 0)
    {
        if (idCuentaPadre is null or 0)

        {
            var cuentasPadre = await dbContext.Cuentas.Where(c =>
                c.IdCuentaPadre == null &&
                c.IdEmpresa     == idEmpresa).ToListAsync();

            var siguienteNumeroPadre = cuentasPadre.Count > 0
                ? cuentasPadre.Max(c => int.Parse(c.Codigo.Split('.')[0])) + 1
                : 1;

            return
                $"{siguienteNumeroPadre}" +
                $"{string.Concat(Enumerable.Repeat(".0", numeroNiveles - 1))}";
        }

        var padre       = await dbContext.Cuentas.FindAsync(idCuentaPadre);
        var codigoPadre = padre.Codigo.Split('.');
        var hijos = await dbContext.Cuentas
                                   .Where(c => c.IdCuentaPadre == idCuentaPadre)
                                   .ToListAsync();
        if (codigoPadre[nivelActual] is not "0")
            return await await Task.FromResult(GenerarCodigo(dbContext, idCuentaPadre,
                idEmpresa,
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