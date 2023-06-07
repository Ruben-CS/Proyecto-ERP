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

    private decimal Total => DetalleCompra.Sum(d => d.PrecioVenta * d.Cantidad);

    private string GetArticuloName(int idArticulo) =>
        Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;

    private void DeleteEntry(DetalleDto loteDto) => DetalleCompra.Remove(loteDto);
}