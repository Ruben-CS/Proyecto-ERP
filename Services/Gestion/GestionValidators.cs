using Microsoft.EntityFrameworkCore;
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


    public async Task<bool> IsValid(GestionDto modeloDto, int idEmpresa)
    {
        var isMasDeDosGestionesActivas =
            await MasDeDosGestionesActivas(modeloDto, idEmpresa);
        var existeNombre        = await ExisteNombre(modeloDto);
        var fechasSonIguales    = await FechasSonIguales(modeloDto);
        var fechasInicioEsMayor = await FechasInicioEsMayor(modeloDto);
        var fechasNoSolapadan   = await FechasNoSolapadan(modeloDto);

        return !isMasDeDosGestionesActivas
               && !existeNombre
               && !fechasSonIguales
               && !fechasInicioEsMayor
               && !fechasNoSolapadan;
    }


    public async Task<bool> MasDeDosGestionesActivas(
        GestionDto gestionDto, int idEmpresa)
    {
        var gestionesActivas = await
            _dbContext.Gestiones.Where(gestion =>
                         gestionDto.IdEmpresa == idEmpresa &&
                         gestion.Estado       == EstadosGestion.Abierto)
                     .ToListAsync();
        if (gestionesActivas is null)
        {
            return false;
        }
        return gestionesActivas.Count == 2;
    }

    public async Task<bool> ExisteNombre(GestionDto gestionDto)
    {
        return await _dbContext.Gestiones.AnyAsync(gestion =>
            gestionDto.Nombre == gestion.Nombre);
    }

    public async Task<bool> FechasSonIguales(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio == gestionDto.FechaFin);
    }

    public async Task<bool> FechasInicioEsMayor(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio > gestionDto.FechaFin);
    }

    public async Task<bool> FechasNoSolapadan(GestionDto gestionDto)
    {
        var gestionesActivas = await _dbContext.Gestiones.Where(gestion =>
            gestionDto.IdEmpresa == gestion.IdEmpresa).ToListAsync();

        return !gestionesActivas.Any(gestion =>
            gestionDto.FechaInicio >= gestion.FechaInicio &&
            gestionDto.FechaInicio <= gestion.FechaFin ||
            gestionDto.FechaFin >= gestion.FechaInicio &&
            gestionDto.FechaFin <= gestion.FechaFin);
    }
}