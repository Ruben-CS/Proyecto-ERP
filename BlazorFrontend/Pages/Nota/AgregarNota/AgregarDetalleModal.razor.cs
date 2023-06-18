using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarDetalleModal
{
    private MudForm? _form;
    public  bool     Succes { get; set; }

    private string? SelectedArticulo { get; set; }

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
    public ObservableCollection<LoteDto> _detalleParaLote { get; set; }

    #endregion

    #region Form Fields

    private int? Cantidad { get; set; }

    private decimal? PrecioUnitario { get; set; }

    private DateTime? FechaVencimiento { get; set; }

    private decimal? SubTotal => Cantidad.HasValue && PrecioUnitario.HasValue
        ? Cantidad.Value * PrecioUnitario.Value
        : null;


    private List<ArticuloDto>? _privateArticulos = new();

    #endregion

    private async Task<IEnumerable<string?>> Search1(string value)
    {
        var nombreArticulos = _privateArticulos!.Select(a => a.Nombre).ToList();
        if (string.IsNullOrEmpty(value))
            return await Task.FromResult(nombreArticulos);
        return nombreArticulos.Where(a => a.Contains(value,
            StringComparison.InvariantCultureIgnoreCase));
    }


    protected override void OnInitialized()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        _privateArticulos                    = Articulos;
    }

    private async Task Submit()
    {
        var articulo =
            _privateArticulos!.SingleOrDefault(a => a.Nombre == SelectedArticulo);
        if (_detalleParaLote.Any(d => d.IdArticulo == articulo.IdArticulo))
        {
            Snackbar.Add("No puede agregar el mismo articulo", Severity.Error);
            return;
        }

        var loteDto = new LoteDto
        {
            Cantidad         = Cantidad!.Value,
            FechaIngreso     = FechaIngreso!.Value,
            Stock            = Cantidad.Value,
            PrecioCompra     = PrecioUnitario!.Value,
            FechaVencimiento = FechaVencimiento,
            IdArticulo       = articulo!.IdArticulo
        };
        await _form!.ResetAsync();
        _form!.ResetValidation();
        await AddNewDetalleLote.InvokeAsync(loteDto);
    }

    private void Cancel() => MudDialog!.Cancel();
}