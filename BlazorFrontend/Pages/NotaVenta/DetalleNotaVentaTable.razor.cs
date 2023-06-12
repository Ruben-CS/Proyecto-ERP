using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.NotaVenta;

public partial class DetalleNotaVentaTable
{
    #region Parameters

    [Parameter]
    public ObservableCollection<DetalleDto> DetalleCompra { get; set; } = new();

    [Parameter]
    public List<ArticuloDto> Articulos { get; set; } = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    private DetalleDto _elementBeforeEdit = null!;

    public string SelectedArticulo { get; set; }

    private decimal Total => DetalleCompra.Sum(d => d.PrecioVenta * d.Cantidad);

    private string StockMessage => $"Stock del lote: {StockDelLoteSeleccionado}";

    private string GetArticuloName(int idArticulo) =>
        Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;

    private IEnumerable<int?>? NroLotes { get; set; }


    private void DeleteEntry(DetalleDto loteDto) => DetalleCompra.Remove(loteDto);

    private List<ArticuloDto>? _privateArticulos = new();

    private List<LoteDto> Lotes { get; set; } = new();

    private int? SelectedNroLote { get; set; }

    private int? Cantidad { get; set; }


    private decimal? PrecioDelArticulo { get; set; }

    private decimal? SubTotal => Cantidad.HasValue && PrecioDelArticulo.HasValue
        ? Cantidad.Value * PrecioDelArticulo.Value
        : null;

    private int StockDelLoteSeleccionado { get; set; }

    private async Task OnArticuloChangeAsync()
    {
        if (!string.IsNullOrEmpty(SelectedArticulo))
        {
            _privateArticulos = Articulos.Where(a => a.Cantidad > 0).ToList();
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
    private void OnRowEditPreview(object detalleObj)
    {

        var idArticulo = Articulos.Single(a => a.Nombre == SelectedArticulo).IdArticulo;
        SelectedArticulo = Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;

        var detalle = detalleObj as DetalleDto;
        _elementBeforeEdit = new DetalleDto
        {
            IdArticulo   = idArticulo,
            NroLote      = detalle.NroLote,
            Cantidad     = detalle.Cantidad,
            PrecioVenta  = detalle.PrecioVenta
        };
    }

    private async Task<IEnumerable<string?>> Search1(string value)
    {
        var nombreArticulos = Articulos.Select(a => a.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
            return await Task.FromResult(nombreArticulos);
        return nombreArticulos.Where(a => a.Contains(value,
            StringComparison.InvariantCultureIgnoreCase));
    }

    private void OnRowEditCommit(object detalleObj)
    {
        StateHasChanged();
    }

    private void OnRowEditCancel(object detalleObj)
    {
        var idArticulo = Articulos.Single(a => a.Nombre == SelectedArticulo).IdArticulo;
        SelectedArticulo = Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;
        var detalle = detalleObj as DetalleDto;
        detalle!.IdArticulo = idArticulo;
        detalle!.NroLote    = _elementBeforeEdit.NroLote;
        detalle.Cantidad    = _elementBeforeEdit.Cantidad;
        detalle.PrecioVenta = _elementBeforeEdit.PrecioVenta;
    }
}