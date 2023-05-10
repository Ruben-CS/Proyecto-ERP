using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
namespace BlazorFrontend.Pages.Dashboard;

public partial class MainPage
{
    [Parameter]
    public int IdEmpresa { get; set; }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri   = new Uri(NavigationManager.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);
            if (query.TryGetValue("id", out var idValue))
            {
                IdEmpresa            = int.Parse(idValue!);
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
}