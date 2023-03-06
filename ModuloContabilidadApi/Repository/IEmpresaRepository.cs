namespace ModuloContabilidadApi.Repository;

public interface IEmpresaRepository<TModelo> where TModelo : class
{
    public Task<IEnumerable<TModelo>> GetModelo();
    public Task<TModelo>              GetModelo(Guid modeloId);
    public Task<TModelo>              CreateModel(TModelo modelo);
    public Task<bool>                 DeleteModel(Guid modeloId);
}