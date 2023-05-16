using MudBlazor;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages;

public partial class InicioSesion
{
    private LoginRequestDto RequestDto { get; } = new();

    private bool IsLoading { get; set; }

    private bool FailedLogin { get; set; }

    private bool _isShow;

    private string _passwordInputIcon = Icons.Material.Filled.VisibilityOff;

    private InputType _passwordInput = InputType.Password;

    private void ButtonTestclick()
    {
        if (_isShow)
        {
            _isShow           = false;
            _passwordInputIcon = Icons.Material.Filled.VisibilityOff;
            _passwordInput     = InputType.Password;
        }
        else {
            _isShow           = true;
            _passwordInputIcon = Icons.Material.Filled.Visibility;
            _passwordInput     = InputType.Text;
        }
    }
    private async Task LoginAsync()
    {
        IsLoading       = true;
        FailedLogin = false;
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
            FailedLogin = false;
        }
        else
        {
            IsLoading       = false;
            FailedLogin = true;
        }
    }
}