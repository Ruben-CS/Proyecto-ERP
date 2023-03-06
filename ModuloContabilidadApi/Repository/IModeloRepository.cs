namespace ModuloContabilidadApi.Repository;

public interface IModeloRepository<TModelo> where TModelo : class
{
    public Task<IEnumerable<TModelo>> GetModelos();
    public Task<TModelo>              GetModelo(Guid modeloId);
    public Task<TModelo>              CreateUpdateModelDto(TModelo modeloDto);
    public Task<bool>                 DeleteModel(Guid modeloId);
}