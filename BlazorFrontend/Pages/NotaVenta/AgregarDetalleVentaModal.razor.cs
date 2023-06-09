using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.NotaVenta;

public partial class AgregarDetalleVentaModal
{
    private MudForm? _form;
    private bool     _succes;

    private string? SelectedArticulo { get; set; }

    private List<LoteDto> Lotes { get; set; } = new();

    private async Task OnArticuloChangeAsync()
    {
        if (!string.IsNullOrEmpty(SelectedArticulo))
        {
            var selectedArticuloDto =
                _privateArticulos.First(a => a.Nombre == SelectedArticulo);
            var lotes =
                await LoteService.GetLotesPerArticleIdAsync(
                    selectedArticuloDto.IdArticulo);
            Lotes             = lotes;
            NroLotes          = lotes.Where(l => l.Stock > 0).Select(l => l.NroLote);
            SelectedNroLote   = NroLotes.First();
            PrecioDelArticulo = selectedArticuloDto.PrecioVenta;
            StockDelLoteSeleccionado =
                lotes.Single(l => l.NroLote == SelectedNroLote).Stock;
        }
        else
        {
            NroLotes          = Enumerable.Empty<int?>();
            PrecioDelArticulo = null;
        }
    }

    private async void UpdateSelectedArticulo(string value)
    {
        SelectedArticulo = value;
        await OnArticuloChangeAsync();
        StateHasChanged();
    }

    #region Parameters

    [Parameter]
    public List<ArticuloDto>? Articulos { get; set; }

    [Parameter]
    public int IdEmpresa { get; set; }

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public EventCallback<DetalleDto> AddNewDetalleLote { get; set; }

    [Parameter]
    public ObservableCollection<DetalleDto> DetalleParaVenta { get; set; } = null!;

    #endregion

    #region Form Fields

    private int? Cantidad { get; set; }

    private decimal? PrecioDelArticulo { get; set; }

    private decimal? SubTotal => Cantidad.HasValue && PrecioDelArticulo.HasValue
        ? Cantidad.Value * PrecioDelArticulo.Value
        : null;


    private List<ArticuloDto>? _privateArticulos = new();

    private IEnumerable<int?>? NroLotes { get; set; }

    private int? SelectedNroLote { get; set; }

    private int StockDelLoteSeleccionado { get; set; }

    #endregion


    private string StockMessage => $"Stock del lote: {StockDelLoteSeleccionado}";

    private async Task<IEnumerable<string?>> Search1(string value)
    {
        var nombreArticulos = _privateArticulos!.Select(a => a.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
            return await Task.FromResult(nombreArticulos);
        return nombreArticulos.Where(a =>
            a!.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    protected override async Task OnInitializedAsync()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        _privateArticulos = Articulos.Where(a => a.Cantidad > 0).ToList();
    }

    private async Task Submit()
    {
        var articulo =
            _privateArticulos!.SingleOrDefault(a => a.Nombre == SelectedArticulo);

        if (Cantidad > Lotes.Single(l => l.NroLote!.Value == SelectedNroLote).Stock)
        {
            Snackbar.Add("La cantidad no puede ser mayor al stock", Severity.Error);
            return;
        }

        if ( DetalleParaVenta.Any(d =>
                d.NroLote == SelectedNroLote && d.IdArticulo == articulo!.IdArticulo))
        {
            Snackbar.Add("No puede agregar el mismo articulo", Severity.Error);
            return;
        }


        var detalleDto = new DetalleDto
        {
            IdArticulo  = articulo!.IdArticulo,
            NroLote     = SelectedNroLote!.Value,
            Cantidad    = Cantidad!.Value,
            PrecioVenta = PrecioDelArticulo!.Value
        };
        Cantidad                 = null;
        PrecioDelArticulo        = null;
        SelectedArticulo         = null;
        SelectedNroLote          = null;
        NroLotes                 = Enumerable.Empty<int?>();
        StockDelLoteSeleccionado = 0;
        await AddNewDetalleLote.InvokeAsync(detalleDto);
    }

    private void Cancel() => MudDialog!.Cancel();
}