using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IComprobanteRepository
{
    public Task<ComprobanteDto> CrearComprobante(ComprobanteDto comprobanteDto,
                                                 int            idEmpresa);

    public Task<IEnumerable<ComprobanteDto>> GetAllComprobantes(int idEmpresa);

    public Task<ComprobanteDto> GetCombrobanteById(int idComprobante);

    public Task<ComprobanteDto> EditComprobante(int idComprobante);

    public Task<bool> DeleteComprobante(int idComprobante);
}