using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
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
		ComprobanteDto comprobanteDto,
		int            idEmpresa)
	{
		var comprobante =
			_mapper.Map<ComprobanteDto, Modelos.Models.Comprobante>(
				comprobanteDto);

		var maxSerie = await _dbContext.Comprobantes
		                               .Where(e => e.IdEmpresa == idEmpresa)
		                               .MaxAsync(e => e.Serie);
		comprobante.Serie = (maxSerie ?? 0) + 1;
		await _dbContext.AddAsync(comprobante);
		await _dbContext.SaveChangesAsync();
		return _mapper.Map<Modelos.Models.Comprobante, ComprobanteDto>(
			comprobante);
	}

	public async Task<List<ComprobanteDto>> GetAllComprobantes(
		int idEmpresa)
	{
		var comprobantes = await _dbContext.Comprobantes
		                                   .Where(c => c.IdEmpresa == idEmpresa)
		                                   .ToListAsync();
		if (comprobantes.IsNullOrEmpty())
		{
			Console.WriteLine("fuck you this is why: ");
		}

		return _mapper.Map<List<ComprobanteDto>>(comprobantes);
	}

	public async Task<ComprobanteDto> GetCombrobanteById(int idComprobante)
	{
		var comprobante = await _dbContext.Comprobantes.Where(c =>
			c.IdComprobante
			== idComprobante).FirstOrDefaultAsync();
		return _mapper.Map<ComprobanteDto>(comprobante);
	}

	public async Task<ComprobanteDto> EditComprobante(int idComprobante)
	{
		var comprobante =
			await _dbContext.Comprobantes.SingleOrDefaultAsync(c =>
				c.IdComprobante == idComprobante);
		return _mapper.Map<ComprobanteDto>(comprobante);
	}

	public async Task<object> AnularComprobante(int idComprobante)
	{
		var comprobante = await _dbContext.Comprobantes
		                                  .Where(c =>
			                                  c.IdComprobante == idComprobante)
		                                  .FirstOrDefaultAsync();
		comprobante!.Estado = EstadoComprobante.Anulado;
		await _dbContext.SaveChangesAsync();
		return await Task.FromResult(true);
	}
}