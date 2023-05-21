using System.Collections.ObjectModel;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class DetalleComprobanteModal
{
    [GeneratedRegex("\\d+(\\.\\d+)*")]
    private static partial Regex MyRegex();

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public EventCallback<DetalleComprobanteDto> AddNewDetalleComprobante { get; set; }

    public List<CuentaDto> Cuentas { get; set; } = new();

    [Parameter]
    public ObservableCollection<DetalleComprobanteDto> Detalles { get; set; } = new();

    #region Fields

    [Parameter]
    public string? Glosa { get; set; }

    private decimal? Debe { get; set; }

    private decimal? Haber { get; set; }

    private string? SelectedCuenta { get; set; }

    #endregion

    private void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        Cuentas                              = await CuentaService.GetCuentasDetalle(IdEmpresa);
    }

    private async Task<IEnumerable<string>> SearchCuenta(string value)
    {
        IEnumerable<string> nombreCuentas = Cuentas.Select(c => $"{c.Codigo} - {c.Nombre}").ToList();
        if (string.IsNullOrEmpty(value))
        {
            return await Task.FromResult(nombreCuentas);
        }
        return nombreCuentas.Where(c => c.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task Submit()
    {
        if (IsDebeOrHaberNull(Debe, Haber))
            return;
        if (ValidateNegatives(Debe!.Value, Haber!.Value))
            return;
        if (IsHaberOrDebeZero(Debe.Value, Haber.Value))
            return;
        if (CuentaHasMoreExistingDetalle())
            return;
        if(IsGlosaNullOrEmpty(Glosa))
            return;

        var selectedCuentaCodigo = ExtractCodigo(SelectedCuenta!);
        var idCuenta             = Cuentas.SingleOrDefault(c => c.Codigo == selectedCuentaCodigo)!.IdCuenta;
        //todo change this to get the id
        var detalleComprobante = new DetalleComprobanteDto
        {
            NombreCuenta = SelectedCuenta,
            Glosa        = Glosa!,
            MontoDebe    = Debe.Value,
            MontoHaber   = Haber.Value,
            IdCuenta     = idCuenta,
            IdUsuario    = 1
        };


        await AddNewDetalleComprobante.InvokeAsync(detalleComprobante);
    }

    private bool ValidateNegatives(decimal debe, decimal haber)
    {
        if (decimal.IsNegative(debe))
        {
            Snackbar.Add("El debe no puede ser negativo", Severity.Error);
            return true;
        }

        if (!decimal.IsNegative(haber)) return false;
        Snackbar.Add("El haber no puede ser negativo", Severity.Error);
        return true;
    }

    private bool IsDebeOrHaberNull(decimal? debe, decimal? haber)
    {
        if (debe is null)
        {
            Snackbar.Add("El campo de debe no puede estar vacio", Severity.Error);
            return true;
        }

        if (haber is not null) return false;
        Snackbar.Add("El campo de haber no puede estar vacio", Severity.Error);
        return true;
    }

    private bool IsHaberOrDebeZero(decimal debe, decimal haber)
    {
        if (debe != 0 && haber != 0)
        {
            Snackbar.Add("El debe o haber tiene que ser 0", Severity.Error);
            return true;
        }

        if (debe == 0 && haber == 0)
        {
            Snackbar.Add("Debe y haber no pueden ser 0 simultaneamente", Severity.Error);
            return true;
        }

        return false;
    }

    private bool CuentaHasMoreExistingDetalle()
    {
        if (!Detalles.Any(d => string.Equals(d.NombreCuenta, SelectedCuenta))) return false;
        Snackbar.Add("No se puede agregar otro detalle a la misma cuenta", Severity.Error);
        return true;
    }

    private bool IsGlosaNullOrEmpty(string? glosa)
    {
        if (!glosa.IsNullOrEmpty()) return false;
        Snackbar.Add("La glosa no puede estar vacia", Severity.Error);
        return true;

    }

    private static string ExtractCodigo(string nombreCuenta)
    {
        var regex = MyRegex();
        var match = regex.Match(nombreCuenta);
        return match.Success ? match.Value : string.Empty;
    }
}