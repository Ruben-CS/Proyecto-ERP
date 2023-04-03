using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using Services.Gestion;
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
            await _applicationDbContext.Gestiones.Where(id => id.IdGestion == modeloId)
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