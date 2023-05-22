using Microsoft.AspNetCore.JsonPatch;
using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IEmpresaMonedaRepository
{
    public Task<List<EmpresaMonedaDto>> GetEmpresasMonedas(int idEmpresa);

    public Task<EmpresaMonedaDto> GetEmpresaMoneda(int id);

    public Task<EmpresaMonedaDto> CreateEmpresaMoneda(EmpresaMonedaDto empresaMonedaDto,
                                                      int              idEmpresa, int idMoneda);

    public Task<EmpresaMonedaDto> CrearMonedaAlternativa(EmpresaMonedaDto empresaMonedaDto,
                                                         int              idEmpresa,
                                                         int              idMonedaAlterna,
                                                         int              idMonedaPrincipal);

    public Task<EmpresaMonedaDto?> UpdateMoneda(
        JsonPatchDocument<EmpresaMonedaDto> patchDocument, int id);

    public Task<List<EmpresaMonedaDto>> GetMonedaAlternativasPerEmpresa(int idEmpresa);
}