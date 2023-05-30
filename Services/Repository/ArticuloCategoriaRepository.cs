using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class ArticuloCategoriaRepository : IArticuloCategoriaRepository
{
    private readonly ApplicationDbContext _dbContext;
    private readonly IMapper              _mapper;

    public ArticuloCategoriaRepository(ApplicationDbContext dbContext, IMapper mapper)
    {
        _dbContext = dbContext;
        _mapper    = mapper;
    }

    public async Task<ArticuloCategoriaDto> CreateArticuloCategoria(
        ArticuloCategoriaDto dto, int idArticulo, int idCategoria)
    {
        var articuloCat = _mapper.Map<ArticuloCategoriaDto, ArticuloCategoria>(dto);
        articuloCat.IdArticulo  = idArticulo;
        articuloCat.IdCategoria = idCategoria;
        await _dbContext.AddAsync(articuloCat);
        await _dbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<ArticuloCategoria, ArticuloCategoriaDto>(articuloCat));
    }

    public async Task<IEnumerable<ArticuloCategoriaDto>> GetArticuloDetalles(
        int idArticulo)
    {
        var articuloDetalles = await _dbContext.ArticuloCategoria.Where(ac =>
            ac.IdArticulo == idArticulo).ToListAsync();
        return await Task.FromResult(
            _mapper.Map<List<ArticuloCategoriaDto>>(articuloDetalles));
    }

    public async Task<ArticuloCategoriaDto> EditArticuloCategoria(
        ArticuloCategoriaDto dto, int idArticulo, int idCategoria)
    {
        var articuloCategoria = await _dbContext.ArticuloCategoria
                                                .Where(ac =>
                                                    ac.IdArticulo  == idArticulo &&
                                                    ac.IdCategoria == idCategoria)
                                                .SingleOrDefaultAsync();
        if (articuloCategoria is null)
        {
            throw new NullReferenceException("ArticuloCategoria no encontrada");
        }
        _mapper.Map(dto,articuloCategoria);
        ;_dbContext.Entry(articuloCategoria).State = EntityState.Modified;
        await _dbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<ArticuloCategoriaDto>(articuloCategoria));
    }
}