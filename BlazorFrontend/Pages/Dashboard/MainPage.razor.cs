using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
namespace BlazorFrontend.Pages.Dashboard;

public partial class MainPage
{
    [Parameter]
    public int Id { get; set; }

    private bool IsExpanded { get; set; }

    bool _open;

    protected override Task OnInitializedAsync()
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

        return Task.CompletedTask;
    }

    private void ToggleDrawer() => _open = !_open;

    private void CambiarEmpresa() => NavigationManager.NavigateTo("/inicio");

    private void NavigateToGestiones()
    {
        if (Id is 0) return;
        var uri = $"/gestion/overview/{Id}";
        NavigationManager.NavigateTo(uri);
    }

    private void NavigateToCuentas()
    {
        if(Id is 0) return;
        var uri = $"/plandecuentas/overview/{Id}";
        NavigationManager.NavigateTo(uri);
    }
}