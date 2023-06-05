using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarDetalleModal
{
    private MudForm? _form;
    private bool     _succes;

    private string SelectedArticulo { get; set; } = string.Empty;

    #region Parameters

    [Parameter]
    public List<ArticuloDto>? Articulos { get; set; }

    [Parameter]
    public int IdEmpresa { get; set; }

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public EventCallback<LoteDto> AddNewDetalleLote { get; set; }

    [Parameter]
    public DateTime? FechaIngreso { get; set; }

    [Parameter]
    public int NroLote { get; set; }

    #endregion

    #region Form Fields

    private int Cantidad { get; set; }

    private decimal PrecioUnitario { get; set; }

    private decimal SubTotal => Cantidad * PrecioUnitario;

    #endregion

    private async Task<IEnumerable<string?>> Search1(string value)
    {
        var nombreArticulos = Articulos.Select(a => a.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
            return await Task.FromResult(nombreArticulos);
        return nombreArticulos.Where(a => a.Contains(value,
            StringComparison.InvariantCultureIgnoreCase));
    }


    protected override void OnInitialized()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
    }

    private async Task Submit()
    {
        //Todo validations

        var idArticulo = Articulos.SingleOrDefault(a => a.Nombre == SelectedArticulo)
                                  .IdArticulo;
        var loteDto = new LoteDto
        {
            Cantidad     = Cantidad,
            FechaIngreso = FechaIngreso!.Value,
            Stock        = Cantidad,
            NroLote      = NroLote,
            PrecioCompra = PrecioUnitario,
            IdArticulo = idArticulo
        };
        await AddNewDetalleLote.InvokeAsync(loteDto);
    }

    private void Cancel() => MudDialog!.Cancel();
}