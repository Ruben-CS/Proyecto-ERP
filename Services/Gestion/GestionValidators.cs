using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;

namespace Services.Gestion;

public static class GestionValidators
{
    private static _applicationDbContext _dbContext = null!;
    public static async Task<bool> ExisteNombre(GestionDto gestionDto, int idEmpresa)
    {
        return await _dbContext.Gestiones.AnyAsync(gestion =>
            gestionDto.Nombre == gestion.Nombre && gestion.IdEmpresa == idEmpresa);
    }

    public static async Task<bool> FechasNoSolapadan(GestionDto gestionDto)
    {
        try
        {
            var gestionesActivas = await _dbContext.Gestiones.Where(gestion =>
                gestionDto.IdEmpresa == gestion.IdEmpresa).ToListAsync();

            if (gestionesActivas is null || gestionesActivas.Count == 0)
            {
                return false;
            }
            else
            {
                var overlapping = gestionesActivas.Any(gestion =>

                    gestionDto.FechaInicio >= gestion.FechaInicio &&

                    gestionDto.FechaInicio <= gestion.FechaFin ||

                    gestionDto.FechaFin >= gestion.FechaInicio &&

                    gestionDto.FechaFin <= gestion.FechaFin);

                return overlapping;

            }
        }
        catch (Exception e)
        {
            throw new NullReferenceException(e.Message);
        }
    }
}