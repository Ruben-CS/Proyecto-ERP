using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.EmpresaMonedaService;

public sealed class EmpresaMonedaService
{
    private readonly HttpClient _httpClient;

    public EmpresaMonedaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmpresaMonedaDto>> GetEmpresasMonedaAsync(int idEmpresa) =>
        await await Task.FromResult(GetApiResponseAsync<List<EmpresaMonedaDto>>
            ($"https://localhost:44352/empresaMonedas/getEmpresasMonedaByIdEmpresa/{idEmpresa}"));

    public async Task<List<EmpresaMonedaDto>> GetEmpresaMonedasActiveAsync(int idEmpresa) =>
        await await Task.FromResult(
            GetApiResponseAsync<List<EmpresaMonedaDto>>(
                $"https://localhost:44352/empresaMonedas/getActiveEmpresaMoneda/{idEmpresa}"));

    private async Task<T> GetApiResponseAsync<T>(string url)
    {
        var response = await _httpClient.GetAsync(url);
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseDto>(content);
        if (responseObject.IsSuccess)
        {
            return await Task.FromResult(
                JsonConvert.DeserializeObject<T>(responseObject.Result.ToString()));
        }

        throw new Exception(string.Join(", ", responseObject.ErrorMessages));
    }
}