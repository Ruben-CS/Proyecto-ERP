using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.Comprobante;

public sealed class ComprobanteService
{
    private readonly HttpClient _httpClient;

    public ComprobanteService(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<List<ComprobanteDto>> GetComprobantesAsync(int idEmpresa) =>
        await await Task.FromResult(GetApiResponseAsync<List<ComprobanteDto>>
            ($"https://localhost:44352/getcomprobantes/{idEmpresa}"));

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