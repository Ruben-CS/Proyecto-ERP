using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using MudBlazor;
namespace BlazorFrontend.Pages.Dashboard;

public partial class MainPage
{
    [Parameter]
    public int Id { get; set; }

    bool _open;
    bool _isExpanded;

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri   = new Uri(NavigationManager.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);
            if (query.TryGetValue("id", out var idValue))
            {
                Id         = int.Parse(idValue!);
                StateHasChanged();
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

    private void NavigateToGestiones()
    {
        if (Id is not 0)
        {
            var uri = $"/gestion/overview/{Id}";
            NavigationManager.NavigateTo(uri);
        }
        else
        {
            Snackbar.Add("Seleccione una empresa antes de continuar.", Severity.Info);
        }
    }
}