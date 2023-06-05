using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.CategoriaService;

public sealed class CategoriaService
{
    private readonly HttpClient _httpClient;

    public CategoriaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CategoriaDto>> GetCategoriasService(int idEmpresa) =>
        await GetApiResponseAsync<List<CategoriaDto>>($"https://localhost:44321/categorias/getCategorias/{idEmpresa}");

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