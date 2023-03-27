using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;

namespace Services.Gestion;

public abstract class GestionValidators
{
    private static readonly ApplicationDbContext DbContext = null!;

    private GestionValidators()
    {
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


    public static async Task<bool> MasDeDosGestionesActivas(
        GestionDto gestionDto, int idEmpresa)
    {
        var gestionesActivas = await
            DbContext.Gestiones.Where(gestion =>
                         gestionDto.IdEmpresa == idEmpresa &&
                         gestion.Estado       == EstadosGestion.Abierto)
                     .ToListAsync();
        return gestionesActivas.Count == 2;
    }

    public static async Task<bool> ExisteNombre(GestionDto gestionDto)
    {
        return await DbContext.Gestiones.AnyAsync(gestion =>
            gestionDto.Nombre == gestion.Nombre);
    }

    public static async Task<bool> FechasSonIguales(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio == gestionDto.FechaFin);
    }

    public static async Task<bool> FechasInicioEsMayor(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio > gestionDto.FechaFin);
    }

    public static async Task<bool> FechasNoSolapadan(GestionDto gestionDto)
    {
        var gestionesActivas = await DbContext.Gestiones.Where(gestion =>
            gestionDto.IdEmpresa == gestion.IdEmpresa).ToListAsync();

        return !gestionesActivas.Any(gestion =>
            gestionDto.FechaInicio >= gestion.FechaInicio &&
            gestionDto.FechaInicio <= gestion.FechaFin ||
            gestionDto.FechaFin >= gestion.FechaInicio &&
            gestionDto.FechaFin <= gestion.FechaFin);
    }
}