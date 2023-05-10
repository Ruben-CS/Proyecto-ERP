using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface ICuentaRepository
{
    public Task<CuentaDto> CreateCuenta(CuentaDto cuentaDto);

    public Task<bool> DeleteCuenta(int id);

    public Task<IEnumerable<CuentaDto>> CreateDefaultCuentas(int idEmpresa);
    public       Task<CuentaDto>              EditCuenta(CuentaDto     cuentaDto, int id);

    public Task<IEnumerable<CuentaDto>> GetAllCuentas(int idempresa);
}