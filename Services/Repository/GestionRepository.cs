using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Gestion;
using Modelos.Models;
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

    public async Task<GestionDto> UpdateModel(GestionDto modeloDto, int idModelo)
    {
        var gestion =
            await _applicationDbContext.Gestiones.SingleAsync(e =>
                e.IdGestion == idModelo);
        if (gestion is null)
        {
            throw new NullReferenceException("Gestion no encontrada");
        }

        _mapper.Map(modeloDto, gestion);

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
            return await Task.FromResult(true);
        }
        catch (Exception e)
        {
            return false;
        }
    }
}