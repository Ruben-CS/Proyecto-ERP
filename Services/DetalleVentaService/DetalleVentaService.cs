using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.DetalleVentaService;

public sealed class DetalleVentaService
{
    private readonly HttpClient _httpClient;

    public DetalleVentaService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<DetalleDto>> GetDetalleComprobantesAsync(
        int idComprobante) =>
        await await Task.FromResult(GetApiResponseAsync<List<DetalleDto>>
            ($"https://localhost:44352/detalleComprobantes/getDetallesById/{idComprobante}"));

    private async Task<T> GetApiResponseAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseDto>(content);
        if (responseObject is not null && responseObject.IsSuccess)
        {
            return JsonConvert.DeserializeObject<T>(responseObject.Result.ToString());
        }

        throw new Exception(string.Join(", ", responseObject.ErrorMessages));
    }
}