using AutoMapper;
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
        ArticuloCategoriaDto dto)
    {
        var articuloCat = _mapper.Map<ArticuloCategoriaDto, ArticuloCategoria>(dto);
        await _dbContext.AddAsync(articuloCat);
        await _dbContext.SaveChangesAsync();
        return await Task.FromResult(
            _mapper.Map<ArticuloCategoria, ArticuloCategoriaDto>(articuloCat));
    }
}