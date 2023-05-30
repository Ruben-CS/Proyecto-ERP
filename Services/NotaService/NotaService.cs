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

    private async Task<T> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonSerializer.Deserialize<ResponseDto>(content);

        if (responseObject!.IsSuccess)
        {
            // If Result is not null, attempt to deserialize it to the expected type.
            // Otherwise, create a new instance of the expected type.
            return responseObject.Result != null
                ? JsonSerializer.Deserialize<T>(responseObject.Result.ToString())
                : Activator.CreateInstance<T>();
        }

        if(responseObject?.ErrorMessages != null)
        {
            throw new Exception(string.Join(", ", responseObject.ErrorMessages));
        }

        throw new Exception("Unable to process the response.");
    }
}