namespace ModuloContabilidadApi.Repository;

public interface IModeloRepository<TModelo> where TModelo : class
{
    public Task<IEnumerable<TModelo>> GetModelo(Guid modeloId);
    public Task<TModelo>              GetModelos();
    public Task<TModelo>              CreateModel(TModelo modelo);
    public Task<bool>                 DeleteModel(Guid modeloId);
}