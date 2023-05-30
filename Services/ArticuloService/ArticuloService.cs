using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.ArticuloService;

public sealed class ArticuloService
{
    private readonly HttpClient _httpClient;

    public ArticuloService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ArticuloDto>> GetArticulosAsync(int idEmpresa) =>
        await GetApiResponseAsync<List<ArticuloDto>>
            ($"https://localhost:44321/articulos/getaArticulos/{idEmpresa}");

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