using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.Models;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class EmpresaRepository : IEmpresaRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public EmpresaRepository(ApplicationDbContext applicationDbContext,
                             IMapper              mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<IEnumerable<EmpresaDto>> GetModelos()
    {
        var listaEmpresas = await _applicationDbContext.Empresas.ToListAsync();
        return _mapper.Map<List<EmpresaDto>>(listaEmpresas);
    }

    public async Task<IEnumerable<EmpresaDto>> GetNonDeletedModels()
    {
        var activeEmpresas = await _applicationDbContext.Empresas
                                                        .Where(e => e.IsDeleted == false)
                                                        .AsNoTracking()
                                                        .ToListAsync();
        return await Task.FromResult(_mapper.Map<List<EmpresaDto>>(activeEmpresas));
    }

    public async Task<EmpresaDto> GetModelo(int modeloId)
    {
        var empresa = await _applicationDbContext.Empresas
                                                 .AsNoTracking()
                                                 .Where(id => id.IdEmpresa == modeloId)
                                                 .FirstOrDefaultAsync();
        return await Task.FromResult(_mapper.Map<EmpresaDto>(empresa));
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
        if (empresa is null)
        {
            throw new NullReferenceException("Empresa no encontrada");
        }

        _applicationDbContext.Entry(empresa).State = EntityState.Detached;

        var updatedEmpresa = _mapper.Map<Empresa>(modeloDto);

        _applicationDbContext.Entry(updatedEmpresa).State = EntityState.Modified;

        await _applicationDbContext.SaveChangesAsync();

        return _mapper.Map<EmpresaDto>(updatedEmpresa);
    }

    public async Task<bool> UpdateIntegracion(EmpresaDto dto, int idEmpresa)
    {
        var empresa = await _applicationDbContext.Empresas.FirstOrDefaultAsync(x =>
            x.IdEmpresa == idEmpresa && x.IsDeleted == false);

        if (empresa == null) return false;
        empresa.Cuenta1                            = dto.Cuenta1;
        empresa.Cuenta2                            = dto.Cuenta2;
        empresa.Cuenta3                            = dto.Cuenta3;
        empresa.Cuenta4                            = dto.Cuenta4;
        empresa.Cuenta5                            = dto.Cuenta5;
        empresa.Cuenta6                            = dto.Cuenta6;
        empresa.Cuenta7                            = dto.Cuenta7;
        _applicationDbContext.Entry(empresa).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return true;

    }

    public async Task<bool> CambiarEstadoDeIntegracionTrue(EmpresaDto  dto, int idEmpresa)
    {
        var empresa = _applicationDbContext.Empresas.FirstOrDefault(x =>
            x.IdEmpresa == idEmpresa && x.IsDeleted == false);
        empresa.TieneIntegracion = true;
        _applicationDbContext.Empresas.Attach(empresa);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> CambiarEstadoDeIntegracionFalse(EmpresaDto dto, int idEmpresa)
    {
        var empresa = _applicationDbContext.Empresas.FirstOrDefault(x =>
            x.IdEmpresa == idEmpresa && x.IsDeleted == false);
        empresa.TieneIntegracion = false;
        _applicationDbContext.Empresas.Attach(empresa);
        await _applicationDbContext.SaveChangesAsync();
        return true;
    }

    public async Task<bool> DeleteModel(int modeloId)
    {
        try
        {
            var empresa = await _applicationDbContext.Empresas
                                                     .FirstOrDefaultAsync(e =>
                                                         e.IdEmpresa == modeloId);
            if (empresa is null)
            {
                return false;
            }

            empresa.IsDeleted = true;
            await _applicationDbContext.SaveChangesAsync();
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }
}