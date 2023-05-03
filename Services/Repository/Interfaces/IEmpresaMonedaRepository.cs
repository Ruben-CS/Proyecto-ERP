using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IEmpresaMonedaRepository
{
    public Task<List<EmpresaMonedaDto>> GetEmpresasMonedas(int idEmpresa);

    public Task<EmpresaMonedaDto> GetEmpresaMoneda(int id);

    public Task<EmpresaMonedaDto> CreateEmpresaMoneda(EmpresaMonedaDto empresaMonedaDto);
}