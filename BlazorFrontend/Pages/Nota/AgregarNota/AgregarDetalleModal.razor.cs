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
    public DateTime? FechaVencimiento { get; set; }

    #endregion

    #region Form Fields

    private int Cantidad { get; set; }

    private decimal PrecioUnitario { get; set; }

    private decimal SubTotal { get; set; }

    private string SubtotalString
    {
        get => SubTotal.ToString("F2");
        set => SubTotal = decimal.TryParse(value, out var result) ? result : decimal.Zero;
    }
    #endregion

    private async Task<IEnumerable<string?>> Search1(string value)
    {
        var nombreArticulos = Articulos.Select(a => a.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
            return await  Task.FromResult(nombreArticulos);
        return nombreArticulos.Where(a => a.Contains(value,
            StringComparison.InvariantCultureIgnoreCase));
    }


    protected override void OnInitialized()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
    }

    private void Submit()
    {
        //Todo validations

        var loteDto = new LoteDto()
        {
            Cantidad = Cantidad,
        };
    }

    private void Cancel() => MudDialog!.Cancel();
}