using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;

namespace Services.Gestion;

public class GestionValidators
{
    private readonly ApplicationDbContext _dbContext;

    public GestionValidators(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }


    public async Task<bool> EsValido(GestionDto modeloDto, int idEmpresa)
    {
        var validationResults = new List<bool>
        {
            await MasDeDosGestionesActivas(idEmpresa),
            await ExisteNombre(modeloDto, idEmpresa),
            await FechasSonIguales(modeloDto),
            await FechasInicioEsMayor(modeloDto),
            await FechasNoSolapadan(modeloDto)
        };
        return !validationResults.Contains(true);
    }


    public async Task<bool> MasDeDosGestionesActivas(int idEmpresa)
    {
        var gestionesActivas = await
            _dbContext.Gestiones.Where(gestion =>
                          gestion.IdEmpresa == idEmpresa &&
                          gestion.Estado    == EstadosGestion.Abierto)
                      .ToListAsync();
        return gestionesActivas.Count >= 2;
    }

    public async Task<bool> ExisteNombre(GestionDto gestionDto, int idEmpresa)
    {
        return await _dbContext.Gestiones.AnyAsync(gestion =>
            gestionDto.Nombre == gestion.Nombre && gestion.IdEmpresa == idEmpresa);
    }

    public static async Task<bool> FechasSonIguales(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio == gestionDto.FechaFin);
    }

    public static async Task<bool> FechasInicioEsMayor(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio > gestionDto.FechaFin);
    }


    public async Task<bool> FechasNoSolapadan(GestionDto gestionDto)
    {
        var gestionesActivas = await _dbContext.Gestiones.Where(gestion =>
            gestionDto.IdEmpresa == gestion.IdEmpresa).ToListAsync();

        bool overlapping = gestionesActivas.Any(gestion =>
            gestionDto.FechaInicio >= gestion.FechaInicio &&
            gestionDto.FechaInicio <= gestion.FechaFin ||
            gestionDto.FechaFin >= gestion.FechaInicio &&
            gestionDto.FechaFin <= gestion.FechaFin);
        return overlapping;
    }
}