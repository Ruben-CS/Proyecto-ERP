using Modelos.Models.Dtos;

namespace NET7.BlazorServerApp.Services;

public static class GetEmpresas
{
    public static async Task<List<EmpresaDto>?> GetEmpresasAsync()
    {
        using var httpClient = new HttpClient();
        var response = await httpClient.GetAsync("https://localhost:44378/empresas/ListarEmpresa");
        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<List<EmpresaDto>>();
    }
}