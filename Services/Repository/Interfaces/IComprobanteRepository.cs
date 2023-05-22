using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IComprobanteRepository
{
    public Task<ComprobanteDto> CrearComprobante(ComprobanteDto comprobanteDto,
                                                 int            idEmpresa);

    public Task<List<ComprobanteDto>> GetAllComprobantes(int idEmpresa);

    public Task<ComprobanteDto> GetCombrobanteById(int idComprobante);

    public Task<ComprobanteDto> EditComprobante(int idComprobante);

    public Task<object> AnularComprobante(int idComprobante);
}