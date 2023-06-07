using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IEmpresaRepository
{
    public Task<IEnumerable<EmpresaDto>> GetModelos();
    public Task<IEnumerable<EmpresaDto>> GetNonDeletedModels();
    public Task<EmpresaDto>              GetModelo(int                   modeloId);
    public Task<EmpresaDto>              CreateUpdateModelDto(EmpresaDto modeloDto);
    public Task<bool>                    DeleteModel(int                 modeloId);
    public Task<EmpresaDto>              UpdateModelDto(EmpresaDto       modeloDto);

    public Task<bool> UpdateIntegracion(EmpresaDto dto,int idEmpresa);

    public Task<bool> CambiarEstadoDeIntegracionTrue(EmpresaDto dto, int idEmpresa);

    public Task<bool> CambiarEstadoDeIntegracionFalse(EmpresaDto dto, int idEmpresa);

}