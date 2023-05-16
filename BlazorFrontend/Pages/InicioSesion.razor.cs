using MudBlazor;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages;

public partial class InicioSesion
{
    private LoginRequestDto RequestDto { get; } = new();

    private bool IsLoading { get; set; }

    public bool SuccessfulLogin { get; set; } = true;
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
            Snackbar.Add($"Bienvenido {loginRequestDto.Nombre}!", Severity.Success,
                o =>
                {
                    o.VisibleStateDuration   = 1500;
                    o.HideTransitionDuration = 350;
                    o.ShowTransitionDuration = 350;
                });
            await LocalStorage.SetItemAsync("username", loginRequestDto.Nombre);
            NavigationManager.NavigateTo("/Inicio");
            IsLoading       = false;
            SuccessfulLogin = true;
        }
        else
        {
            IsLoading       = false;
            SuccessfulLogin = false;
        }
    }
}