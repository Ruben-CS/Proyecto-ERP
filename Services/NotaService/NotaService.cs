using Modelos.Models.Dtos;
using System.Text.Json;
using Newtonsoft.Json;

namespace Services.NotaService;

public sealed class NotaService
{
    private readonly HttpClient _httpClient;

    public NotaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<NotaDto>?> GetNotaComprasAsync(int idEmpresa) =>
        await GetAsync<List<NotaDto>>(
            $"https://localhost:44321/notas/getNotaCompras/{idEmpresa}");

    public async Task<List<NotaDto>?> GetNotaVentasAsync(int idEmpresa) =>
        await GetAsync<List<NotaDto>>(
            $"https://localhost:44321/notas/getNotaVenta/{idEmpresa}");

    public async Task<NotaDto> GetNotaAsync(int idNota) =>
        await GetAsync<NotaDto>(
            $"https://localhost:44321/notas/getNota/{idNota}");

    private async Task<T> GetAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseDto>(content);

        if (responseObject!.IsSuccess)
        {
            // If Result is not null, attempt to deserialize it to the expected type.
            // Otherwise, create a new instance of the expected type.
            if (responseObject.Result != null)
            {
                var result =
                    JsonConvert.DeserializeObject<T>(responseObject.Result.ToString());
                return result;
            }
            else
            {
                return default(T);
            }
        }

        if (responseObject?.ErrorMessages != null)
        {
            throw new Exception(string.Join(", ", responseObject.ErrorMessages));
        }

        throw new Exception("Unable to process the response.");
    }
}