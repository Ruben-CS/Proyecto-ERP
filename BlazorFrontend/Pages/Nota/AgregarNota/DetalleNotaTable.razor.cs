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

    private decimal Total => DetalleCompra.Sum(d => d.PrecioCompra * d.Cantidad);

    private string GetArticuloName(int idArticulo) =>
        Articulos.Single(a => a.IdArticulo == idArticulo).Nombre!;
}