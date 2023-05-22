using Microsoft.AspNetCore.Components;
using MudBlazor;
using Modelos.Models.Dtos;
using BlazorFrontend.Pages.Periodo.Crear;
using BlazorFrontend.Pages.Periodo.Editar;
using BlazorFrontend.Pages.Periodo.Eliminar;
using Microsoft.JSInterop;

namespace BlazorFrontend.Pages.Periodo;

public partial class InicioPeriodo
{
    [Parameter]
    public int IdGestion { get; set; }


    [Parameter]
    public int IdEmpresa { get; set; }


    private IEnumerable<PeriodoDto> _periodos   = new List<PeriodoDto>();
    private GestionDto              _gestionDto = new();

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
            var uri = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out var id))
            {
                _gestionDto = await GestionServices.GetGestionSingleAsync(id);
                _periodos   = await PeriodoService.GetPeriodosAsync(IdGestion);
                IdEmpresa   = Convert.ToInt32(segments[^2]);
            }
            else
            {
                throw new ArgumentException
                    ("The 'idgestion' parameter is missing or invalid.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An error occurred while initializing the component: {ex}");
        }
    }

    private async Task EditarPeriodo(PeriodoDto periodoDto)
    {
        var parameters = new DialogParameters
        {
            { "IdPeriodo", periodoDto.IdPeriodo },
            { "IdGestion", periodoDto.IdGestion },
            { "PeriodoDto", periodoDto },
            {
                "OnPeriodoDataGridChange",
                EventCallback.Factory.Create<PeriodoDto>(this, OnPeriodoDataGridChange)
            }
        };
        await DialogService.ShowAsync<EditarPeriodo>(string.Empty, parameters, _options);
    }

    private async Task BorrarPeriodo(PeriodoDto periodoDto)
    {
        var parameters = new DialogParameters
        {
            { "IdPeriodo", periodoDto.IdPeriodo },
            {
                "OnPeriodoDataGridChange",
                EventCallback.Factory.Create<PeriodoDto>(this, OnPeriodoDataGridChange)
            }
        };
        await DialogService.ShowAsync<EliminarPeriodo>(string.Empty, parameters,
            _options);
    }

    private async void ShowMudCrearPeriodonModal()
    {
        var parameters = new DialogParameters
        {
            { "IdGestion", IdGestion },
            { "IdEmpresa", _gestionDto.IdEmpresa },
            {
                "OnPeriodoDataGridChange",
                EventCallback.Factory.Create<PeriodoDto>(this, OnPeriodoDataGridChange)
            }
        };
        await DialogService.ShowAsync<CrearPeriodo>
            ("Llene los datos del periodo", parameters, _options);
    }

    private async Task GoBack()
    {
        await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));
    }

    private async Task OnPeriodoDataGridChange(PeriodoDto periodoDto)
    {
        _periodos = await PeriodoService.GetPeriodosAsync(IdGestion);
        await InvokeAsync(StateHasChanged);
    }

}