using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Gestion;
using Services.Repository.Interfaces;

namespace ModuloContabilidadApi.Repository;

public class GestionRepository : IGestionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;
    private readonly GestionValidators    _gestionValidators;

    public GestionRepository(ApplicationDbContext applicationDbContext,
                             IMapper mapper, GestionValidators gestionValidators)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
        _gestionValidators    = gestionValidators;
    }

    public async Task<IEnumerable<GestionDto>> GetModelos()
    {
        var listaGestiones =
            await _applicationDbContext.Gestiones.ToListAsync();
        return _mapper.Map<List<GestionDto>>(listaGestiones);
    }

    public async Task<GestionDto> GetModelo(int modeloId)
    {
        var gestion = await _applicationDbContext.Gestiones
                                                 .Where(id => id.IdGestion == modeloId)
                                                 .FirstOrDefaultAsync();
        return _mapper.Map<GestionDto>(gestion);
    }

    public async Task<GestionDto> CreateUpdateModelDto(
        GestionDto gestionDto, int idEmpresa)
    {
        var gestion = _mapper.Map<GestionDto, Gestion>(gestionDto);
        try
        {
            if (await _gestionValidators.EsValido(gestionDto, idEmpresa))

            {
                _applicationDbContext.Add(gestion);
                await _applicationDbContext.SaveChangesAsync();
            }
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }
        return _mapper.Map<Gestion, GestionDto>(gestion);
    }

    public async Task<bool> DeleteModel(int modeloId)
    {
        try
        {
            var gestion = await _applicationDbContext.Gestiones
                                                     .FirstOrDefaultAsync(e =>
                                                         e.IdGestion == modeloId);
            if (gestion is null)
            {
                return false;
            }

            gestion.Estado = EstadosGestion.Cerrado;
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}