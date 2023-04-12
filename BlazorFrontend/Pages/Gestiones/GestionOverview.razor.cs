using BlazorFrontend.Pages.Gestiones.Crear;
using BlazorFrontend.Pages.Gestiones.Editar;
using BlazorFrontend.Pages.Gestiones.Eliminar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.WebUtilities;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;
using DialogOptions = MudBlazor.DialogOptions;

namespace BlazorFrontend.Pages.Gestiones;

public partial class GestionOverview
{
    private IEnumerable<GestionDto> _gestiones =
        new List<GestionDto>().Where(x => x.Estado == EstadosGestion.Abierto);

    [Inject]
    ISnackbar Snackbar { get; set; } = null !;

    [Parameter]
    public int Id { get; set; }

    private readonly DialogOptions _options = new()
    {
        CloseOnEscapeKey     = true,
        MaxWidth             = MaxWidth.Small,
        FullWidth            = true,
        DisableBackdropClick = true
    };

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri   = new Uri(NavigationManager.Uri);
            var query = QueryHelpers.ParseQuery(uri.Query);
            if (query.TryGetValue("id", out var idValue))
            {
                Id         = int.Parse(idValue!);
                _gestiones = await GestionServices.GetGestionAsync(Id);
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

    private async void ShowMudCrearGestionModal()
    {
        var parameters = new DialogParameters
        {
            { "Id", Id }
        };
        var result = await DialogService.ShowAsync<CrearGestion>
            ("Llene los datos de la gestion", parameters, _options);

        if (result.Result == null) return;
        _gestiones = await GestionServices.GetGestionAsync(Id);
        StateHasChanged();
    }


    private void EditarGestion(GestionDto item)
    {
        var parameters = new DialogParameters
        {
            {
                "Id",
                item.IdGestion
            },
            {
                "GestionDto",
                item
            }
        };
        DialogService.ShowAsync<EditarGestion>("Editar gestion", parameters,
            _options);
    }

    private async Task BorrarGestion(GestionDto item)
    {
        var parameters = new DialogParameters
        {
            {
                "Id",
                item.IdGestion
            }
        };
        await DialogService.ShowAsync<EliminarGestion>("Editar gestion", parameters,
            _options);
    }

    private void NavigateToPage(GestionDto gestion)
    {
        if (gestion.IdGestion is not 0)
        {
            var uri = $"/overview/inicioperiodo/{gestion.IdGestion}";
            NavigationManager.NavigateTo(uri);
        }
        else
        {
            Snackbar.Add("Seleccione una empresa antes de continuar.", Severity.Info);
        }
    }
}