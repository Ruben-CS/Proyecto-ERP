using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class ArticuloRepository : IArticuloRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public ArticuloRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<ArticuloDto> CrearArticulo(ArticuloDto dto)
    {
        var articulo = _mapper.Map<ArticuloDto, Articulo>(dto);
        await _applicationDbContext.AddAsync(articulo);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Articulo, ArticuloDto>(articulo);
    }

    public async Task<IEnumerable<ArticuloDto>> ListarArticulo(int idEmpresa)
    {
        var articulos = await _applicationDbContext.Articulo
                                                   .Where(a => a.IdEmpresa == idEmpresa)
                                                   .ToListAsync();
        return await Task.FromResult(_mapper.Map<List<ArticuloDto>>(articulos));
    }

    public async Task<ArticuloDto> EditarArticulo(ArticuloDto dto, int idArticulo)
    {
        var articulo =
            await _applicationDbContext.Articulo.Where(c =>
                c.IdArticulo == idArticulo).SingleOrDefaultAsync();

        if (articulo is null)
        {
            throw new NullReferenceException("Articulo no encontrado");
        }

        dto.IdEmpresa = articulo.IdEmpresa;
        _mapper.Map(dto, articulo);
        _applicationDbContext.Entry(articulo).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(_mapper.Map<ArticuloDto>(articulo));
    }

    //Todo fix this security risk goddamit but it'll do for now
    public async Task<bool> EditarCantidadArticulo(int idArticulo, int cantidad)
    {
        var articulo =
            await _applicationDbContext.Articulo.SingleOrDefaultAsync(a =>
                a.IdArticulo == idArticulo);
        if (articulo is null)
        {
            return await Task.FromResult(false);
        }

        articulo.Cantidad                           += cantidad;
        _applicationDbContext.Entry(articulo).State =  EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<ArticuloDto> GetSingleArticulo(int idArticulo)
    {
        var articulo =
            await _applicationDbContext.Articulo.FirstOrDefaultAsync(c =>
                c.IdArticulo == idArticulo);
        return await Task.FromResult(_mapper.Map<ArticuloDto>(articulo));
    }

    public async Task<bool> BorrarArticulo(int idArticulo)
    {
        var aritculo =
            await _applicationDbContext.Articulo.SingleOrDefaultAsync(c =>
                c.IdArticulo == idArticulo);

        if (aritculo is null)
        {
            return await Task.FromResult(false);
        }

        _applicationDbContext.Remove(aritculo);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }
}