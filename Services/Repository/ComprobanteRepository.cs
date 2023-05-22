using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class ComprobanteRepository : IComprobanteRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper              _mapper;

    public ComprobanteRepository(ApplicationDbContext dbContext,
                                 IMapper              mapper)
    {
        _dbContext = dbContext;
        _mapper    = mapper;
    }

    public async Task<ComprobanteDto> CrearComprobante(
        ComprobanteDto comprobanteDto, int idEmpresa)
    {
        var comprobante = _mapper.Map<ComprobanteDto, Modelos.Models.Comprobante>(comprobanteDto);

        var maxSerie = await _dbContext.Comprobantes
                                       .Where(e =>
                                           e.IdEmpresa == idEmpresa)
                                       .MaxAsync(e => e.Serie);

        comprobante.Serie = (maxSerie ?? 0) + 1;
        await _dbContext.AddAsync(comprobante);
        await _dbContext.SaveChangesAsync();
        return _mapper.Map<Modelos.Models.Comprobante, ComprobanteDto>(comprobante);
    }

    public async Task<List<ComprobanteDto>> GetAllComprobantes(
        int idEmpresa)
    {
        var comprobantes =  await _dbContext.Comprobantes.Where(c => c.IdEmpresa == idEmpresa).ToListAsync();
        if (comprobantes.IsNullOrEmpty())
        {
            Console.WriteLine("fuck you this is why: ");
        }
        return _mapper.Map<List<ComprobanteDto>>(comprobantes);
    }

    public Task<ComprobanteDto> GetCombrobanteById(int idComprobante)
    {
        throw new NotImplementedException();
    }

    public Task<ComprobanteDto> EditComprobante(int idComprobante)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteComprobante(int idComprobante)
    {
        throw new NotImplementedException();
    }
}