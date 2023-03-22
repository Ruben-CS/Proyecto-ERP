using Modelos.Models.Dtos;

namespace ModuloContabilidadApi.Repository.Interfaces;

public interface IEmpresaRepository
{
    public Task<IEnumerable<EmpresaDto>> GetModelos();
    public Task<EmpresaDto> GetModelo(int modeloId);
    public Task<EmpresaDto> CreateUpdateModelDto(EmpresaDto modeloDto);
    public Task<bool> DeleteModel(int modeloId);

    public Task<EmpresaDto> UpdateModelDto(EmpresaDto modeloDto);
}