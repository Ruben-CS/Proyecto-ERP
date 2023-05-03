using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Services.Cuenta;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class CuentaRepository : ICuentaRepository
{
    private readonly ApplicationDbContext _applicationDbContext;

    private readonly IMapper _mapper;

    public CuentaRepository(ApplicationDbContext applicationDbContext, IMapper mapper)
    {
        _applicationDbContext = applicationDbContext;
        _mapper               = mapper;
    }

    public async Task<CuentaDto> CreateCuenta(CuentaDto cuentaDto)
    {

        var selectedEmpresa =
            await _applicationDbContext.Empresas.FirstOrDefaultAsync(e =>
                e.IdEmpresa == cuentaDto.IdEmpresa);

        var levelsPerEmpresa = Convert.ToInt32(selectedEmpresa!.Niveles);

        cuentaDto.Codigo = await CuentaUtility.GenerarCodigo
            (_applicationDbContext, cuentaDto.IdCuentaPadre,
                cuentaDto.IdEmpresa, levelsPerEmpresa);
        var cuenta = _mapper.Map<CuentaDto, Modelos.Models.Cuenta>(cuentaDto);
        await _applicationDbContext.AddAsync(cuenta);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Modelos.Models.Cuenta, CuentaDto>(cuenta);
    }

    public async Task<bool> DeleteCuenta(int id)
    {
        var cuenta = await _applicationDbContext.Cuentas.FirstOrDefaultAsync(c => c
            .IdCuenta == id);
        if (cuenta is null)
        {
            return await Task.FromResult(false);
        }

        _applicationDbContext.Remove(cuenta);
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(true);
    }

    public async Task<CuentaDto> EditCuenta(CuentaDto cuentaDto, int id)
    {
        var cuenta = await _applicationDbContext.Cuentas.SingleAsync(
            c => c.IdCuenta == id);
        if (cuenta is null)
        {
            throw new NullReferenceException("Cuenta no encontrada");
        }

        cuentaDto.IdEmpresa = cuenta.IdEmpresa;

        _mapper.Map(cuentaDto, cuenta);

        _applicationDbContext.Entry(cuenta).State = EntityState.Modified;
        await _applicationDbContext.SaveChangesAsync();
        return await Task.FromResult(_mapper.Map<CuentaDto>(cuenta));
    }

    public async Task<IEnumerable<CuentaDto>> GetAllCuentas(int idempresa)
    {
        var listaGestiones =
            await _applicationDbContext.Cuentas
                                       .Where(id => id.IdEmpresa == idempresa)
                                       .ToListAsync();
        return _mapper.Map<List<CuentaDto>>(listaGestiones);
    }

}