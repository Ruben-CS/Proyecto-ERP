namespace ModuloContabilidadApi.Repository;

public interface IModeloRepository<TModeloDto> where TModeloDto : class
{
    public Task<IEnumerable<TModeloDto>> GetModelos();
    public Task<TModeloDto>              GetModelo(Guid modeloId);
    public Task<TModeloDto>              CreateUpdateModelDto(TModeloDto modeloDto);
    public Task<bool>                 DeleteModel(Guid modeloId);
}