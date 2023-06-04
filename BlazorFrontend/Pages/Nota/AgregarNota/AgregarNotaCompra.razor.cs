using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarNotaCompra
{
    private bool     _success;
    private MudForm? _form;
    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Nota Details

    private string? NroNota { get; set; }

    private string? Descripcion { get; set; }

    private DateTime? Fecha { get; set; } = DateTime.Today;
    #endregion

    private List<NotaDto>? _notas { get; set; } = new();

    private List<ArticuloDto> Articulos { get; set; } = new();

    private ObservableCollection<LoteDto> DetalleParaLote = new();
    private async Task GoBack() =>
        await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));

    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        _notas    = await NotaService.GetNotaComprasAsync(IdEmpresa);
        var nextNro = GetNextNumeroNota();
        NroNota = nextNro.ToString();
        await InvokeAsync(StateHasChanged);
    }

    private int GetNextNumeroNota()
    {
        var maxNroNota = _notas.Max(n => n.NroNota);
        return (maxNroNota ?? 0) + 1;
    }

    private async Task OpenAgregarDetalle()
    {
        var parameters = new DialogParameters
        {
            { "FechaVencimiento", Fecha },
            { "Articulos", Articulos },
            { "IdEmpresa", IdEmpresa },
            {"AddNewDetalleLote", EventCallback.Factory.Create<LoteDto>(this, AddNewDetalleLote) }
        };

        await DialogService.ShowAsync<AgregarDetalleModal>("Ingrese los detalles", parameters,_options);
    }

    private void AddNewDetalleLote(LoteDto lote)
    {
        DetalleParaLote.Add(lote);
    }
}