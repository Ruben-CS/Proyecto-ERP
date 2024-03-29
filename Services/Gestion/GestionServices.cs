using Modelos.Models.Dtos;
using Newtonsoft.Json;


namespace Services.Gestion;

public sealed class GestionServices
{
    private readonly HttpClient _httpClient;

    public GestionServices(HttpClient httpClient) => _httpClient = httpClient;

    public async Task<GestionDto> GetGestionSingleAsync(int id) =>
        await GetApiResponseAsync<GestionDto>
            ($"https://localhost:44378/gestiones/gestion/id={id}");

    public async Task<List<GestionDto>> GetGestionAsync(int id) =>
        await GetApiResponseAsync<List<GestionDto>>
            ($"https://localhost:44378/gestiones/ListarGestion/id={id}");

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