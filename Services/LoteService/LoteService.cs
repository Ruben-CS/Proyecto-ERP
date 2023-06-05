using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.LoteService;

public sealed class LoteService
{
    private readonly HttpClient _httpClient;

    public LoteService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<LoteDto>?> GetLotesAsync(int idNota) =>
        await GetAsync<List<LoteDto>>(
            $"https://localhost:44321/lotes/getLotes/{idNota}");

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