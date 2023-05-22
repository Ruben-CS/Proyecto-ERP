using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IDetalleComprobanteRepository
{
    public Task<object> CreateDetalleComprobante(
        DetalleComprobanteDto detalleComprobanteDto);

    public Task<object> GetComprobantes(int idComprobante);
}