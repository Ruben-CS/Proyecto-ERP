using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace BlazorFrontend.Services;

public sealed class EmpresaService
{
    private readonly HttpClient _httpClient;
    public EmpresaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmpresaDto>> GetEmpresasAsync()
    {
        return await GetApiResponseAsync<List<EmpresaDto>>("https://localhost:44378/empresas/ListarEmpresa");
    }

    public async Task<EmpresaDto?> GetEmpresaByIdAsync(int id)
    {
        return await GetApiResponseAsync<EmpresaDto>($"https://localhost:44378/empresas/{id}");
    }

    private async Task<T> GetApiResponseAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseDto>(content);
        if (responseObject.IsSuccess)
        {
            return JsonConvert.DeserializeObject<T>(responseObject.Result.ToString());
        }

        throw new Exception(string.Join(", ", responseObject.ErrorMessages));
    }
}