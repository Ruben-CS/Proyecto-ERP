using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Modelos.ApplicationContexts;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
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

        var esDetalle = cuentaDto.Codigo.Split('.').Last() == "0";

        cuentaDto.TipoCuenta = esDetalle ? TipoCuenta.Global : TipoCuenta.Detalle;

        var cuenta = _mapper.Map<CuentaDto, Modelos.Models.Cuenta>(cuentaDto);
        await _applicationDbContext.AddAsync(cuenta);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Modelos.Models.Cuenta, CuentaDto>(cuenta);
    }

    public async Task<IEnumerable<CuentaDto>> CreateDefaultCuentas(int idEmpresa)
    {
        var defaultCuentas = new List<CuentaDto>
        {
            new() { Nombre = "Activo", TipoCuenta     = TipoCuenta.Global, IdEmpresa = idEmpresa },
            new() { Nombre = "Pasivo", TipoCuenta     = TipoCuenta.Global, IdEmpresa = idEmpresa },
            new() { Nombre = "Patrimonio", TipoCuenta = TipoCuenta.Global, IdEmpresa = idEmpresa },
            new() { Nombre = "Ingresos", TipoCuenta   = TipoCuenta.Global, IdEmpresa = idEmpresa },
            new() { Nombre = "Egresos", TipoCuenta    = TipoCuenta.Global, IdEmpresa = idEmpresa }
        };

        var createdCuentas = new List<CuentaDto>();

        foreach (var defaultCuenta in defaultCuentas)
        {
            var createdCuentaDto = await CreateCuenta(defaultCuenta);
            createdCuentas.Add(createdCuentaDto);
        }

        var egresosDto = createdCuentas.Last();
        var subCuentasEgresos = new List<CuentaDto>
        {
            new()
            {
                Nombre    = "Costos", IdCuentaPadre = egresosDto.IdCuenta,
                IdEmpresa = idEmpresa
            },
            new()
            {
                Nombre    = "Gastos", IdCuentaPadre = egresosDto.IdCuenta,
                IdEmpresa = idEmpresa
            }
        };

        foreach (var subCuenta in subCuentasEgresos)
        {
            var createdSubCuentaDto = await CreateCuenta(subCuenta);
            createdCuentas.Add(createdSubCuentaDto);
        }

        return createdCuentas;
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

    public async Task<IEnumerable<CuentaDto>> GetCuentasTipoDetalle(int idEmpresa)
    {
        var cuentasDetalle = await _applicationDbContext.Cuentas.Where(c => c.IdEmpresa == idEmpresa)
                                                        .ToListAsync();
        return _mapper.Map<List<CuentaDto>>(cuentasDetalle);
    }
}