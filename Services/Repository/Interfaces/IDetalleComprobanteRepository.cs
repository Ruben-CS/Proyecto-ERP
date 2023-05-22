using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IDetalleComprobanteRepository
{
    public Task<object> CreateDetalleComprobante(int idComprobante,
                                                 DetalleComprobanteDto detalleComprobanteDto);
}