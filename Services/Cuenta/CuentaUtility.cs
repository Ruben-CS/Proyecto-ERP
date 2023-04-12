using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;

namespace Services.Cuenta;

internal static class CuentaUtility
{
    private static readonly ApplicationDbContext ApplicationDbContext = null!;

    public static async Task<string> GenerarCodigo(int? idCuentaPadre, int idEmpresa,
                                                   int numeroNiveles, int nivelActual = 0)
    {
        if (idCuentaPadre is null or 0)
        {
            var cuentasPadre = await ApplicationDbContext.Cuentas.Where(c =>
                c.IdCuentaPadre == null && c.IdEmpresa == idEmpresa).ToListAsync();

            var siguienteNumeroPadre = cuentasPadre.Count > 0
                ? cuentasPadre.Max(c => int.Parse(c.Codigo.Split('.')[0])) + 1
                : 1;
            return $"{siguienteNumeroPadre}" +
                   string.Concat(Enumerable.Repeat(".0", numeroNiveles - 1));
        }

        var padre       = await ApplicationDbContext.Cuentas.FindAsync(idCuentaPadre);
        var codigoPadre = padre.Codigo.Split('.');
        var hijos = await ApplicationDbContext.Cuentas
                                              .Where(c =>
                                                  c.IdCuentaPadre == idCuentaPadre)
                                              .ToListAsync();
        if (codigoPadre[nivelActual] is "0")
        {
            var siguienteNumeroHijo = hijos.Count > 0
                ? hijos.Max(c => int.Parse(c.Codigo.Split('.')[nivelActual])) + 1
                : 1;
            codigoPadre[nivelActual] = $"{siguienteNumeroHijo}";
            return await Task.FromResult(string.Join(".", codigoPadre));
        }

        return await await Task.FromResult(GenerarCodigo(idCuentaPadre, idEmpresa,
            numeroNiveles,
            nivelActual + 1));
    }
}