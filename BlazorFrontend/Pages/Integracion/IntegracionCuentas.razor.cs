using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Integracion;

public partial class IntegracionCuentas
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private List<CuentaDto> Cuentas { get; set; } = new();

    private bool _configurar;

    private bool Configurar
    {
        get => _configurar;
        set
        {
            if (_configurar != value)
            {
                _configurar = value;
                OnCheckChanged(value);
            }
        }
    }

    private EmpresaDto Empresa { get; set; } = new();

    #region Fields

    private string? Cuenta1 { get; set; }
    private string? Cuenta2 { get; set; }

    private string? Cuenta3 { get; set; }

    private string? Cuenta4 { get; set; }

    private string? Cuenta5 { get; set; }

    private string? Cuenta6 { get; set; }

    private string? Cuenta7 { get; set; }

    #endregion

    private async Task OnCheckChanged(bool newValue)
    {
        Configurar = newValue;

        if (!Configurar)
        {
            var uriDesactivar =
                $"https://localhost:44378/integracion/desactivarConfiguracion/{IdEmpresa}";
            await HttpClient.PutAsJsonAsync(uriDesactivar, Empresa);
            Snackbar.Add("Integracion desactivada", MudBlazor.Severity.Info);
        }
        else
        {
            var uriActivar =
                $"https://localhost:44378/integracion/activarConfiguracion/{IdEmpresa}"; // Note: You should change 'desactivarConfiguracion' to 'activarConfiguracion' in this URL.
            await HttpClient.PutAsJsonAsync(uriActivar, Empresa);
            Snackbar.Add("Integracion activada", MudBlazor.Severity.Info);
        }
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa   = int.Parse(idValue);
                Cuentas     = await CuentaService.GetCuentasDetalle(IdEmpresa);
                Empresa     = (await EmpresaService.GetEmpresaByIdAsync(IdEmpresa))!;
                Configurar  = Empresa.TieneIntegracion!.Value;
                Cuenta1     = GetNombreCuenta(Empresa.Cuenta1.Value);
                Cuenta2     = GetNombreCuenta(Empresa.Cuenta2.Value);
                Cuenta3     = GetNombreCuenta(Empresa.Cuenta3.Value);
                Cuenta4     = GetNombreCuenta(Empresa.Cuenta4.Value);
                Cuenta5     = GetNombreCuenta(Empresa.Cuenta5.Value);
                Cuenta6     = GetNombreCuenta(Empresa.Cuenta6.Value);
                Cuenta7     = GetNombreCuenta(Empresa.Cuenta7.Value);
                _configurar = Empresa.TieneIntegracion.Value;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                throw new KeyNotFoundException(
                    "The 'id' parameter was not found in the query string.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An error occurred while initializing the component: {ex}");
        }
    }

    private async Task GuardarConfiguracion()
    {
        var properties = new[]
        {
            Cuenta1, Cuenta2, Cuenta3, Cuenta4, Cuenta5, Cuenta6, Cuenta7
        };
        var codigosCuenta = new List<int>();
        var uri = $"https://localhost:44378/integracion/agregarConfiguracion/{IdEmpresa}";

        if (properties.Any(string.IsNullOrEmpty))
        {
            Snackbar.Add("No pueden haber campos vacios", MudBlazor.Severity.Error);
            return;
        }

        if (properties.Distinct().Count() != properties.Length)
        {
            Snackbar.Add("No pueden haber campos duplicados", MudBlazor.Severity.Error);
            return;
        }

        foreach (var nombreCuentas in properties)
        {
            var codigoCuenta = ExtractCodigo(nombreCuentas!);
            var idCuenta = Cuentas.SingleOrDefault(c => c.Codigo == codigoCuenta)!
                                  .IdCuenta;
            codigosCuenta.Add(idCuenta);
        }

        Empresa.Cuenta1 = codigosCuenta.First();
        Empresa.Cuenta2 = codigosCuenta[1];
        Empresa.Cuenta3 = codigosCuenta[2];
        Empresa.Cuenta4 = codigosCuenta[3];
        Empresa.Cuenta5 = codigosCuenta[4];
        Empresa.Cuenta6 = codigosCuenta[5];
        Empresa.Cuenta7 = codigosCuenta.Last();

        var response = await HttpClient.PutAsJsonAsync(uri, Empresa);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Configuracion guardada exitosamente",
                MudBlazor.Severity.Success);
        }
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

    private string GetNombreCuenta(int? idCuenta)
    {
        var nombreCuenta = Cuentas
                           .Where(C => C.IdCuenta == idCuenta)
                           .Select(c => $"{c.Codigo} - {c.Nombre}")
                           .FirstOrDefault();
        return nombreCuenta ?? string.Empty;
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