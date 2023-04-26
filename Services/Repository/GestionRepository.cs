using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class GestionRepository : IGestionRepository
{
    private readonly ApplicationDbContext _applicationDbContext;
    private readonly IMapper              _mapper;

    public GestionRepository(ApplicationDbContext applicationDbContext,
                             IMapper              mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<IEnumerable<GestionDto>> GetModelos(int modeloId)
    {
        var listaGestiones =
            await _applicationDbContext.Gestiones.Where(id => id.IdEmpresa == modeloId)
                                       .ToListAsync();
        return _mapper.Map<List<GestionDto>>(listaGestiones);
    }

    public async Task<GestionDto> CreateUpdateModelDto(
        GestionDto gestionDto, int idEmpresa)
    {
        var gestion = _mapper.Map<GestionDto, Modelos.Models.Gestion>(gestionDto);
        //todo check why this is not inserting from the url
        gestion.IdEmpresa = idEmpresa;
        try
        {
            _applicationDbContext.Add(gestion);
            await _applicationDbContext.SaveChangesAsync();
        }
        catch (Exception e)
        {
            Console.WriteLine(e);
            throw;
        }

        return _mapper.Map<Modelos.Models.Gestion, GestionDto>(gestion);
    }

    public async Task<GestionDto> UpdateModel(GestionDto gestionDto, int idGestion)
    {
        var gestion =
            await _applicationDbContext.Gestiones.SingleAsync(e =>
                e.IdGestion == idGestion);
        if (gestion is null)
        {
            throw new NullReferenceException("Gestion no encontrada");
        }

        _mapper.Map(gestionDto, gestion);

        _applicationDbContext.Entry(gestion).State = EntityState.Modified;

        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(_mapper.Map<GestionDto>(gestion));
    }

    public async Task<GestionDto> GetModelo(int modeloId)
    {
        var empresa = await _applicationDbContext.Gestiones.SingleAsync(id => id
            .IdGestion == modeloId);
        return await Task.FromResult(_mapper.Map<GestionDto>(empresa));
    }

    public async Task<bool> DeleteModel(int modeloId)
    {
        var gestion = await _applicationDbContext
                            .Gestiones
                            .FirstOrDefaultAsync(e => e.IdGestion == modeloId);
        if (gestion is null)
        {
            return false;
        }
        _applicationDbContext.Remove(gestion);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}