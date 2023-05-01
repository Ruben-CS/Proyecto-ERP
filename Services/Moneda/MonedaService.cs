using System.Text.Json;
using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.Moneda;

public sealed class MonedaService
{
    private readonly HttpClient _httpClient;

    public MonedaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<MonedaDto>?> GetMonedasAsync()
    {
        return await Task.FromResult(
            await GetApiResponseAsync<List<MonedaDto>>(
                "https://localhost:44352/monedas/getMonedas"));
    }

    private async Task<T?> GetApiResponseAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseDto>(content);
        if (responseObject.IsSuccess)
        {
            return await Task.FromResult(JsonConvert.DeserializeObject<T>
                (responseObject.Result.ToString()));
        }

        throw new Exception(string.Join(", ", responseObject.ErrorMessages));
    }
}