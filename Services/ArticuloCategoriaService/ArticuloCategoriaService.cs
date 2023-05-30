using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.ArticuloCategoriaService;

public sealed class ArticuloCategoriaService
{
    private readonly HttpClient _httpClient;

    public ArticuloCategoriaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<ArticuloCategoriaDto>> GetArticuloCategoriasAsync(int idArticulo) =>
        await await Task.FromResult(GetArticuloCategoriasAsync<List<ArticuloCategoriaDto>>
            ($"https://localhost:44321/articuloCategoria/getArticuloDetalles/{idArticulo}"));

    private async Task<T> GetArticuloCategoriasAsync<T>(string url)
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