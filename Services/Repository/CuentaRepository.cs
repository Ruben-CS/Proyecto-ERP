using Modelos.Models.Dtos;
using Services.Repository.Interfaces;

namespace Services.Repository;

public class CuentaRepository : ICuentaRepository
{
    public Task<CuentaDto> CreateCuenta(CuentaDto cuenta)
    {
        throw new NotImplementedException();
    }

    public Task<bool>      DeleteCuenta(int       id)
    {
        throw new NotImplementedException();
    }

    public Task<CuentaDto> EditCuenta(CuentaDto   cuentaDto)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<CuentaDto>>          GetAllCuentas()
    {
        throw new NotImplementedException();
    }
}