using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.DetalleComprobante;

public class DetalleComprobanteService
{
    private readonly HttpClient _httpClient;

    public DetalleComprobanteService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<DetalleComprobanteDto>> GetDetalleComprobantesAsync(
        int idComprobante) =>
        await await Task.FromResult(GetApiResponseAsync<List<DetalleComprobanteDto>>
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