using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ICuentaRepository
{
    public Task<CuentaDto> CreateCuenta(CuentaDto cuenta);

    public Task<bool> DeleteCuenta(int id);

    //todo check valid method
    public Task<CuentaDto> EditCuenta(CuentaDto cuentaDto);

    public Task<IEnumerable<CuentaDto>> GetAllCuentas();
}