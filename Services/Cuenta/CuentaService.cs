using Modelos.Models.Dtos;
using Newtonsoft.Json;

namespace Services.Cuenta;

public sealed class CuentaService
{
    private readonly HttpClient _httpClient;

    public CuentaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<CuentaDto>> GetCuentasAsync(int id) =>
        await await Task.FromResult(GetApiResponseAsync<List<CuentaDto>>(
            $"https://localhost:44378/cuentas/getcuentas/{id}"));

    public async Task<List<CuentaDto>> GetCuentasDetalle(int idEmpresa) =>
        await await Task.FromResult(GetApiResponseAsync<List<CuentaDto>>($"https://localhost:44378/cuentas/getCuentasDetalle/{idEmpresa}"));

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