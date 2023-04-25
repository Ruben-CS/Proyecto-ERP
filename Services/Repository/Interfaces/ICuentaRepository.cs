using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ICuentaRepository
{
    public Task<CuentaDto> CreateCuenta(CuentaDto cuentaDto);

    public Task<bool> DeleteCuenta(int id);

    //todo check valid method
    public Task<CuentaDto> EditCuenta(CuentaDto cuentaDto, int id);

    public Task<IEnumerable<CuentaDto>> GetAllCuentas(int idempresa);
}