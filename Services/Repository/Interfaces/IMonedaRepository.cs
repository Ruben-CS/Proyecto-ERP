using Modelos.Models.Dtos;

namespace Services.Repository.Interfaces;

public interface IMonedaRepository
{
    public Task<IEnumerable<MonedaDto>> GetAllMonedas();
    public Task<MonedaDto>              GetMoneda(int    idMoneda);
    public Task<bool>                   DeleteMoneda(int idMoneda);

    public Task<MonedaDto> CreateMoneda(MonedaDto monedaDto,
                                        int       idEmpresa);

    public Task<MonedaDto> UpdateMoneda(MonedaDto monedaDto, int idMoneda);
}