using System.Net.Http.Headers;
using Modelos.Models.Dtos;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;

namespace NET7.BlazorServerApp.Services;

public class EmpresaService
{
    private readonly HttpClient _httpClient;
    public EmpresaService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }

    public async Task<List<EmpresaDto>> GetEmpresasAsync()
    {
        var response = await _httpClient.GetAsync("https://localhost:44378/empresas/ListarEmpresa");
        response.EnsureSuccessStatusCode();

        var content        = await response.Content.ReadAsStringAsync();
        var responseObject = JsonConvert.DeserializeObject<ResponseDto>(content);
        if (responseObject.IsSuccess)
        {
            return JsonConvert.DeserializeObject<List<EmpresaDto>>(responseObject.Result.ToString());
        }

        throw new Exception(string.Join(", ", responseObject.ErrorMessages));
    }
}