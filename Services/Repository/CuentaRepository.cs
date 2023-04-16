using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
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
            (_applicationDbContext, cuentaDto.IdCuentaPadre, cuentaDto.IdEmpresa, levelsPerEmpresa);
        var cuenta = _mapper.Map<CuentaDto, Modelos.Models.Cuenta>(cuentaDto);
        await _applicationDbContext.AddAsync(cuenta);
        await _applicationDbContext.SaveChangesAsync();
        return _mapper.Map<Modelos.Models.Cuenta, CuentaDto>(cuenta);
    }

    public Task<bool> DeleteCuenta(int id)
    {
        throw new NotImplementedException();
    }

    public Task<CuentaDto> EditCuenta(CuentaDto cuentaDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CuentaDto>> GetAllCuentas()
    {
        throw new NotImplementedException();
    }

}