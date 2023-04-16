using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;

namespace BlazorFrontend.Pages.Cuentas;

public partial class CuentasOverview
{
    private bool IsExpanded { get; set; }

    private bool _open;

    [Parameter]
    public int IdEmpresa { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa  = int.Parse(idValue);
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                throw new KeyNotFoundException(
                    "The 'id' parameter was not found in the query string.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An error occurred while initializing the component: {ex}");
        }
    }

    private void ToggleDrawer() => _open = !_open;

    private void CambiarEmpresa() => NavigationManager.NavigateTo("/inicio");

}