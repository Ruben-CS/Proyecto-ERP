using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class CategoriaRepository : ICategoriaRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public CategoriaRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<CategoriaDto> CreateCategoria(CategoriaDto dto)
    {
        var categoria = _mapper.Map<CategoriaDto, Categoria>(dto);

        await _applicationDbContext.AddAsync(categoria);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Categoria, CategoriaDto>(categoria);
    }

    public async Task<CategoriaDto> EditarDto(CategoriaDto dto, int idCategoria)
    {
        var categoria =
            await _applicationDbContext.Categorias.Where(c =>
                c.IdCategoria == idCategoria).SingleOrDefaultAsync();

        if (categoria is null)
        {
            throw new NullReferenceException("Cuenta no encontrada");
        }

        dto.IdEmpresa = categoria.IdEmpresa;
        _mapper.Map(dto, categoria);
        _applicationDbContext.Entry(categoria).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(_mapper.Map<CategoriaDto>(categoria));
    }

    public async Task<bool> EliminarDto(int idCategoria)
    {
        var categoria = await _applicationDbContext.Categorias.SingleOrDefaultAsync(c => c.IdCategoria == idCategoria);

        if (categoria is null)
        {
            return await Task.FromResult(false);
        }

        _applicationDbContext.Remove(categoria);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<IEnumerable<CategoriaDto>> ListarCategoria(int idEmpresa)
    {
        var cuentas = await _applicationDbContext.Categorias
                                                 .Where(c => c.IdEmpresa == idEmpresa)
                                                 .ToListAsync();

        return await Task.FromResult(_mapper.Map<List<CategoriaDto>>(cuentas));
    }

    public async Task<CategoriaDto> GetCategoriaById(int idCategoria)
    {
        var categoria =
            await _applicationDbContext.Categorias.FirstOrDefaultAsync(c =>
                c.IdCategoria == idCategoria);
        return await Task.FromResult(_mapper.Map<CategoriaDto>(categoria));
    }
}