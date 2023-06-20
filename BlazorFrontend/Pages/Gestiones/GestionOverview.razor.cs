using BlazorFrontend.Pages.Gestiones.Crear;
using BlazorFrontend.Pages.Gestiones.Editar;
using BlazorFrontend.Pages.Gestiones.Eliminar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;
using Services.Periodo;
using DialogOptions = MudBlazor.DialogOptions;

namespace BlazorFrontend.Pages.Gestiones;

public partial class GestionOverview
{
    private List<GestionDto> _gestiones = new();

    [Inject]
    private ISnackbar Snackbar { get; set; } = null !;

    [Parameter]
    public int IdEmpresa { get; set; }

    private IEnumerable<PeriodoDto> _periodosPorGestion = new List<PeriodoDto>();

    [Inject]
    private IJSRuntime JSRuntime { get; set; }


    private bool VerifyGestiones()
    {
        var count = _gestiones.Count(gestion =>
            gestion.IdEmpresa == IdEmpresa
            && gestion.Estado == EstadosGestion.Abierto);
        return count == 2;
    }

    private const bool Click = false;
    private const bool Focus = false;

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
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa           = int.Parse(idValue);
                _gestiones          = await GestionServices.GetGestionAsync(IdEmpresa);
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

    private async void ShowMudCrearGestionModal()
    {
        var parameters = new DialogParameters
        {
            { "Id", IdEmpresa },
            {
                "OnDataGridChange",
                EventCallback.Factory.Create<GestionDto>(this, OnDataGridChange)
            }
        };

        var result = await DialogService.ShowAsync<CrearGestion>
            ("Llene los datos de la gestion", parameters, _options);

        if (result.Result == null) return;
        _gestiones = await GestionServices.GetGestionAsync(IdEmpresa);
    }


    private void EditarGestion(GestionDto item)
    {
        var parameters = new DialogParameters
        {
            { "Id", item.IdGestion },
            { "GestionDto", item },
            {
                "OnDataGridChange",
                EventCallback.Factory.Create<GestionDto>(this, OnDataGridChange)
            }
        };
        DialogService.ShowAsync<EditarGestion>("Editar gestion", parameters,
            _options);
    }

    private async Task BorrarGestion(GestionDto item)
    {
        _periodosPorGestion = await PeriodoService.GetPeriodosAsync(item.IdGestion);

        var gestionConPeriodo = _periodosPorGestion.Where(periodo => periodo.IdGestion
            == item.IdGestion).ToList();

        if (gestionConPeriodo.Count > 0)
        {
            Snackbar.Add("No se puede eliminar la gestion por que tiene periodos activos",
                Severity.Error);
            return;
        }
        var parameters = new DialogParameters
        {
            { "Id", item.IdGestion },
            {
                "OnDataGridChange",
                EventCallback.Factory.Create<GestionDto>(this, OnDataGridChange)
            }
        };
        await DialogService.ShowAsync<EliminarGestion>("Editar gestion", parameters,
            _options);
    }

    private void NavigateToPage(GestionDto gestion)
    {
        if (gestion.IdGestion is not 0)
        {
            var uri = $"/overview/inicioperiodo/{IdEmpresa}/{gestion.IdGestion}";
            NavigationManager.NavigateTo(uri);
        }
        else
        {
            Snackbar.Add("Seleccione una empresa antes de continuar.", Severity.Info);
        }
    }

    private async Task OnDataGridChange(GestionDto gestionDto)
    {
        _gestiones = await GestionServices.GetGestionAsync(IdEmpresa);
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }

    private void GenerateReport()
    {
        var url = $"http://localhost:80/Reports/report/Report%20Project1/ReporteGestiones?IdEmpresa={IdEmpresa}";
        OpenUrlInNewTab(url);
    }
    private void OpenUrlInNewTab(string url)
    {
        var js = $"window.open('{url}', '_blank');";
        JSRuntime.InvokeVoidAsync("eval", js);
    }
}