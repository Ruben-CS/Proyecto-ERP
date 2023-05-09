using MudBlazor;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages;

public partial class InicioSesion
{
    private LoginRequestDto RequestDto { get; } = new();

    public bool IsLoading { get; set; }
    private async Task LoginAsync()
    {
        IsLoading = true;
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
            IsLoading = false;
        }
        else
        {
            Snackbar.Add($"Credenciales incorrectas", Severity.Error);
            IsLoading = false;
        }
    }
}