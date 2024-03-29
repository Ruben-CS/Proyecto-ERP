using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class DetalleComprobanteModal
{

    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public EventCallback<DetalleComprobanteDto> AddNewDetalleComprobante { get; set; }

    [Parameter]
    public bool IsMonedaPrincipal { get; set; }

    [Parameter]
    public decimal TipoDeCambio { get; set; }

    public List<CuentaDto> Cuentas { get; set; } = new();

    [Parameter]
    public ObservableCollection<DetalleComprobanteDto> Detalles { get; set; } = new();

    #region Fields

    [Parameter]
    public string? Glosa { get; set; }

    private decimal Debe { get; set; } = decimal.Zero;

    private decimal Haber { get; set; } = decimal.Zero;

    private string? SelectedCuenta { get; set; }

    #endregion

    private void Cancel() => MudDialog.Cancel();

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
        Cuentas = await CuentaService.GetCuentasDetalle(IdEmpresa);
    }

    private async Task<IEnumerable<string>> SearchCuenta(string value)
    {
        IEnumerable<string> nombreCuentas =
            Cuentas.Select(c => $"{c.Codigo} - {c.Nombre}").ToList();
        if (string.IsNullOrEmpty(value))
        {
            return await Task.FromResult(nombreCuentas);
        }

        return nombreCuentas.Where(c =>
            c.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task Submit()
    {
        decimal debe;
        decimal haber;
        decimal debeAlt;
        decimal haberAlt;

        if (IsDebeOrHaberNull(Debe, Haber))
            return;
        if (ValidateNegatives(Debe, Haber))
            return;
        if (IsHaberOrDebeZero(Debe, Haber))
            return;
        if (CuentaHasMoreExistingDetalle())
            return;
        if (IsGlosaNullOrEmpty(Glosa))
            return;

        if (IsMonedaPrincipal)
        {
            debe     = Debe;
            haber    = Haber;
            debeAlt  = Debe  / TipoDeCambio;
            haberAlt = Haber / TipoDeCambio;
        }
        else
        {
            debe     = Debe  * TipoDeCambio;
            haber    = Haber * TipoDeCambio;
            debeAlt  = Debe;
            haberAlt = Haber;
        }

        var selectedCuentaCodigo = ExtractCodigo(SelectedCuenta!);
        var idCuenta = Cuentas.SingleOrDefault(c => c.Codigo == selectedCuentaCodigo)!
                              .IdCuenta;
        var detalleComprobante = new DetalleComprobanteDto
        {
            NombreCuenta  = SelectedCuenta,
            Glosa         = Glosa!,
            MontoDebe     = debe,
            MontoHaber    = haber,
            IdCuenta      = idCuenta,
            IdUsuario     = 1,
            MontoHaberAlt = haberAlt,
            MontoDebeAlt  = debeAlt
        };
        await AddNewDetalleComprobante.InvokeAsync(detalleComprobante);
        Debe           = decimal.Zero;
        Haber          = decimal.Zero;
        SelectedCuenta = null;
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
        if (!Detalles.Any(d => string.Equals(d.NombreCuenta, SelectedCuenta)))
            return false;
        Snackbar.Add("No se puede agregar otro detalle a la misma cuenta",
            Severity.Error);
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
        var separatorIndex = nombreCuenta.IndexOf(" - ", StringComparison.Ordinal);

        if (separatorIndex == -1)
            return string.Empty;
        var numberPart = nombreCuenta[..separatorIndex].Trim();

        return IsValidNumberPart(numberPart) ? numberPart : string.Empty;
    }

    private static bool IsValidNumberPart(string numberPart)
    {
        var segments = numberPart.Split('.');
        if (segments.Any(segment => !int.TryParse(segment, out _)))
        {
            return false;
        }

        return segments.Length >= 3 && segments.Length <= 7;
    }
}