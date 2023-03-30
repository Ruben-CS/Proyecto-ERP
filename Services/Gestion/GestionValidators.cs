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


    public async Task<List<string>> EsValido(GestionDto modeloDto, int idEmpresa)
    {
        List<string> errorMessages = new List<string>();

        string multiplesGestionesMessage = await MasDeDosGestionesActivas(idEmpresa);
        if (!string.IsNullOrEmpty(multiplesGestionesMessage))
            errorMessages.Add(multiplesGestionesMessage);

        string existeNombreMessage = await ExisteNombre(modeloDto, idEmpresa);
        if (!string.IsNullOrEmpty(existeNombreMessage))
            errorMessages.Add(existeNombreMessage);
        string fechasSonIgualesMessage = await FechasSonIguales(modeloDto);
        if (!string.IsNullOrEmpty(fechasSonIgualesMessage))
            errorMessages.Add(fechasSonIgualesMessage);

        string fechasInicioEsMayorMessage = await FechasInicioEsMayor(modeloDto);
        if (!string.IsNullOrEmpty(fechasInicioEsMayorMessage))
            errorMessages.Add(fechasInicioEsMayorMessage);

        string fechasNoSolapadanMessage = await FechasNoSolapadan(modeloDto);
        if (!string.IsNullOrEmpty(fechasNoSolapadanMessage))
            errorMessages.Add(fechasNoSolapadanMessage);

        return errorMessages;
    }


    public async Task<string> MasDeDosGestionesActivas(int idEmpresa)
    {
        var gestionesActivas = await
            _dbContext.Gestiones.Where(gestion =>
                          gestion.IdEmpresa == idEmpresa &&
                          gestion.Estado       == EstadosGestion.Abierto)
                      .ToListAsync();
        // if (gestionesActivas.IsNullOrEmpty())
        // {
        //     return "There are no active gestiones.";
        // }

        return gestionesActivas.Count < 2 ? string.Empty: "More than two active";
    }

    public async Task<string> ExisteNombre(GestionDto gestionDto, int idEmpresa)
    {
        return await _dbContext.Gestiones.AnyAsync(gestion =>
            gestionDto.Nombre == gestion.Nombre && gestion.IdEmpresa == idEmpresa) ? "The name already exists." : string.Empty;
    }

    public static async Task<string> FechasSonIguales(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio == gestionDto.FechaFin) ? "The start and end dates are the same." : string.Empty;
    }

    public static async Task<string> FechasInicioEsMayor(GestionDto gestionDto)
    {
        return await Task.Run(() => gestionDto.FechaInicio > gestionDto.FechaFin) ? "The start date is greater than the end date." : string.Empty;
    }


    public async Task<string> FechasNoSolapadan(GestionDto gestionDto)
    {
        var gestionesActivas = await _dbContext.Gestiones.Where(gestion =>
            gestionDto.IdEmpresa == gestion.IdEmpresa).ToListAsync();


        bool overlapping = gestionesActivas.Any(gestion =>
            gestionDto.FechaInicio >= gestion.FechaInicio &&
            gestionDto.FechaInicio <= gestion.FechaFin ||
            gestionDto.FechaFin >= gestion.FechaInicio &&
            gestionDto.FechaFin <= gestion.FechaFin);
        return overlapping ? "The dates overlap with existing gestiones." : string.Empty;
    }
}