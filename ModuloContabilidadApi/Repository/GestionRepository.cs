using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;

namespace ModuloContabilidadApi.Repository;

public class GestionRepository : IGestionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;

    public GestionRepository(ApplicationDbContext applicationDbContext,
                             IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<IEnumerable<GestionDto>> GetModelos()
    {
        var listaGestiones =
            await _applicationDbContext.Gestiones.ToListAsync();
        return _mapper.Map<List<GestionDto>>(listaGestiones);
    }

    public async Task<GestionDto> GetModelo(Guid modeloId)
    {
        var gestion = await _applicationDbContext.Gestiones
            .Where(id => id.IdGestion == modeloId).FirstOrDefaultAsync();
        return _mapper.Map<GestionDto>(gestion);
    }

    public async Task<GestionDto> CreateUpdateModelDto(GestionDto
        gestionDto)
    {
        var gestion = _mapper.Map<GestionDto, Gestion>(gestionDto);
        var existeGesion = await _applicationDbContext.Gestiones
            .FirstOrDefaultAsync(e => e.IdGestion == gestion.IdGestion);

        if (existeGesion is null)
        {
            _applicationDbContext.Update(gestion);
        }
        else
        {
            _applicationDbContext.Add(gestion);
        }

        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Gestion, GestionDto>(gestion);
    }

    public async Task<bool> DeleteModel(Guid modeloId)
    {
        try
        {
            var gestion = _applicationDbContext.Gestiones
                .FirstOrDefaultAsync(e => e.IdGestion == modeloId);
            if (gestion is null)
            {
                return false;
            }
            else
            {
                _applicationDbContext.Remove(gestion);
                await _applicationDbContext.SaveChangesAsync();
                return true;
            }
        }
        catch (Exception e)
        {
            return false;
        }
    }
}