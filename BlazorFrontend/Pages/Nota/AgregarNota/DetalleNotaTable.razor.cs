using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class DetalleNotaTable
{
    #region Parameters

    [Parameter]
    public ObservableCollection<LoteDto> DetalleCompra { get; set; } = new();

    [Parameter]
    public List<ArticuloDto> Articulos { get; set; } = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    private LoteDto _elementBeforeEdit = null!;

    private string? NombreArticulo { get; set; }

    private decimal Total => DetalleCompra.Sum(d => d.PrecioCompra * d.Cantidad);

    private string GetArticuloName(int idArticulo) =>
        Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;

    private void DeleteEntry(LoteDto loteDto) => DetalleCompra.Remove(loteDto);

    private async Task<IEnumerable<string?>> Search1(string value)
    {
        var nombreArticulos = Articulos.Select(a => a.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
            return await Task.FromResult(nombreArticulos);
        return nombreArticulos.Where(a => a.Contains(value,
            StringComparison.InvariantCultureIgnoreCase));
    }

    private void OnRowEditPreview(object detalleObj)
    {
        var idArticulo = Articulos.Single(a => a.Nombre == NombreArticulo).IdArticulo;
        NombreArticulo = Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;

        var detalle = detalleObj as LoteDto;
        _elementBeforeEdit = new LoteDto
        {
            IdArticulo   = idArticulo,
            Cantidad     = detalle!.Cantidad,
            PrecioCompra = detalle.PrecioCompra,
        };
    }

    private void OnRowEditCommit(object detalleObj)
    {
        StateHasChanged();
    }

    private void OnRowEditCancel(object detalleObj)
    {
        var idArticulo = Articulos.Single(a => a.Nombre == NombreArticulo).IdArticulo;
        NombreArticulo = Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;
        var detalle = detalleObj as LoteDto;
        detalle!.IdArticulo  = idArticulo;
        detalle.Cantidad     = _elementBeforeEdit.Cantidad;
        detalle.PrecioCompra = _elementBeforeEdit.PrecioCompra;
    }
}