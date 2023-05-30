using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Nota.DetalleNotaTable;


public partial class DetalleNotaTable
{
    #region Parameters

    [Parameter]
    public ObservableCollection<ArticuloDto> DetalleCompra { get; set; } = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public List<ArticuloDto> Articulos { get; set; } = new();

    #endregion
}