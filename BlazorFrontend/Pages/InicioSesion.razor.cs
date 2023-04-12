using MudBlazor;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages;

public partial class InicioSesion
{
    public  LoginRequestDto RequestDto { get; } = new();
    private bool            _showInvalidCredentials;
    private async Task LoginAsync()
    {
        const string loginUrl = "https://localhost:44378/Auth/login";
        var loginRequestDto = new LoginRequestDto
        {
            Nombre   = RequestDto.Nombre,
            Password = RequestDto.Password
        };
        var response = await HttpClient.PostAsJsonAsync(loginUrl, loginRequestDto);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add($"Bienvenido {loginRequestDto.Nombre}!", Severity.Success);
            NavigationManager.NavigateTo("/Inicio");
        }
        else
        {
            Snackbar.Add($"Credenciales incorrectas", Severity.Error);
            _showInvalidCredentials = true;
        }
    }
}