using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models;
using Modelos.Models.Dtos;
using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;
using ModuloContabilidadApi.Repository.Interfaces;

namespace ModuloContabilidadApi.Repository;

public class EmpresaRepository : IEmpresaRepository
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

    public async Task<EmpresaDto> GetModelo(int modeloId)
    {
        var empresa = await _applicationDbContext.Empresas.Where(id => id
            .IdEmpresa == modeloId).FirstOrDefaultAsync();
        return _mapper.Map<EmpresaDto>(empresa);
    }

    public async Task<EmpresaDto> CreateUpdateModelDto(EmpresaDto
                                                           modeloDto)
    {
        var empresa = _mapper.Map<EmpresaDto, Empresa>(modeloDto);
        _applicationDbContext.Add(empresa);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Empresa, EmpresaDto>(empresa);
    }

    public async Task<EmpresaDto> UpdateModelDto(EmpresaDto modeloDto)
    {
        var empresa =
            await _applicationDbContext.Empresas.FirstOrDefaultAsync(e =>
                e.IdEmpresa == modeloDto.IdEmpresa);
        if (empresa == null)
        {
            throw new Exception(
                $"Empresa with IdEmpresa {modeloDto.IdEmpresa} not found.");
        }

        _applicationDbContext.Entry(empresa).State = EntityState.Detached;

        var updatedEmpresa = _mapper.Map<Empresa>(modeloDto);

        _applicationDbContext.Entry(updatedEmpresa).State = EntityState.Modified;

        await _applicationDbContext.SaveChangesAsync();

        return _mapper.Map<EmpresaDto>(updatedEmpresa);
    }

    public async Task<bool> DeleteModel(int modeloId)
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