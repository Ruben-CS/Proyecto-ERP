using System.Reflection.Metadata;
using AutoMapper;
using Microsoft.EntityFrameworkCore;
using ModuloContabilidadApi.ApplicationContexts;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository;

namespace ModuloContabilidadApi.Repository;
public class EmpresaRepository : IModeloRepository<EmpresaDto>
{
    private readonly ApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public EmpresaRepository(ApplicationDbContext applicationDbContext,
                             IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<IEnumerable<EmpresaDto>> GetModelos()
    {
        var listaEmpresas = await _applicationDbContext.Empresas.ToListAsync();
        return _mapper.Map<List<EmpresaDto>>(listaEmpresas);
    }

    public async Task<EmpresaDto> GetModelo(Guid modeloId)
    {
        var empresa = await _applicationDbContext.Empresas.Where(id => id
            .IdEmpresa == modeloId).FirstOrDefaultAsync();
        return _mapper.Map<EmpresaDto>(empresa);
    }

    public async Task<EmpresaDto> CreateUpdateModelDto(EmpresaDto
        modeloDto)
    {
        var empresa = _mapper.Map<EmpresaDto, Empresa>(modeloDto);
        var existeEmpresa = await _applicationDbContext.Empresas
            .FirstOrDefaultAsync(e => e.IdEmpresa == empresa.IdEmpresa);
        if (existeEmpresa is null)
        {
            _applicationDbContext.Update(empresa);
        }
        else
        {
            _applicationDbContext.Add(empresa);
        }

        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Empresa, EmpresaDto>(empresa);
    }

    public async Task<bool> DeleteModel(Guid modeloId)
    {
        try
        {
            var empresa = await _applicationDbContext.Empresas
                .FirstOrDefaultAsync(e => e.IdEmpresa == modeloId);
            if (empresa is null)
            {
                return false;
            }

            empresa.IsDeleted = true;
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception e)
        {
            return false;
        }
    }
}