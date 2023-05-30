using Modelos.Models.Dtos;
using System.Text.Json;

namespace Services.NotaService;

public sealed class NotaService
{
    private readonly HttpClient _httpClient;

    public NotaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NotaDto>?> GetNotasAsync(int idEmpresa) =>
        await GetAsync<List<NotaDto>>($"https://localhost:44321/notas/getNotas/{idEmpresa}");

    private async Task<T?> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ResponseDto>(content);

        if (responseObject == null)
            throw new Exception("Unable to process the response.");
        if (responseObject.IsSuccess && responseObject.Result != null)
        {
            return JsonSerializer.Deserialize<T>(responseObject.Result.ToString() ??
                                                 string.Empty);
        }
        if (responseObject.ErrorMessages != null)
        {
            throw new Exception(string.Join(", ", responseObject.ErrorMessages));
        }

        throw new Exception("Unable to process the response.");
    }
}