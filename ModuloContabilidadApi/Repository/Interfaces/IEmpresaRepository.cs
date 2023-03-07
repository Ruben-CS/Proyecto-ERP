using ModuloContabilidadApi.Models;
using ModuloContabilidadApi.Models.Dtos;

namespace ModuloContabilidadApi.Repository.Interfaces;

public interface IEmpresaRepository
{
    public Task<IEnumerable<EmpresaDto>> GetModelos();
    public Task<EmpresaDto> GetModelo(Guid modeloId);
    public Task<EmpresaDto> CreateUpdateModelDto(EmpresaDto modeloDto);
    public Task<bool> DeleteModel(Guid modeloId);
}