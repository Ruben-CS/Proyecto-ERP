using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IDetalleRepository
{
    public Task<DetalleDto> AgregarDetalleVenta(DetalleDto dto);
    public Task<bool>       EliminarDetalleVenta(DetalleDto dto);
}