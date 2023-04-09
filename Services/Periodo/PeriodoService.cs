using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.Periodo;

public sealed class PeriodoService
{
    private readonly HttpClient _httpClient;

    public PeriodoService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<PeriodoDto>> GetPeriodosAsync(int id)
    {
        return await GetApiResponseAsync
            <List<PeriodoDto>>($"https://localhost:44378/periodos/ListarPeriodos/{id}");
    }

    private async Task<T> GetApiResponseAsync<T>(string url)
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