using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.NotaVenta;

public partial class AgregarNotaVenta
{
    private bool     _success;
    private MudForm? _form;

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Nota Details

    private string? NroNota { get; set; }

    private string? Descripcion { get; set; }

    private DateTime? Fecha { get; set; } = DateTime.Today;

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; } = new();

    #endregion

    private List<NotaDto> Notas { get; set; } = new();

    private EmpresaDto Empresa { get; set; } = null!;

    private List<ArticuloDto> Articulos { get; set; } = new();

    private readonly ObservableCollection<DetalleDto> _detalleParaVenta = new();

    private async Task GoBack() =>
        await Task.FromResult(
            JSRuntimeExtensions.InvokeVoidAsync(JsRuntime,
                "blazorBrowserHistory.goBack"));

    private void AddNewDetalleLote(DetalleDto lote) => _detalleParaVenta.Add(lote);


    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        await GetNotaVentas();
        var nextNro = GetNextNumeroNota();
        NroNota = nextNro.ToString();
        Empresa = (await EmpresaService.GetEmpresaByIdAsync(IdEmpresa))!;
        await InvokeAsync(StateHasChanged);
    }

    private int GetNextNumeroNota()
    {
        var maxNroNota = Notas.Max(n => n.NroNota);
        return (maxNroNota ?? 0) + 1;
    }

    private async Task OpenAgregarDetalle()
    {
        var parameters = new DialogParameters
        {
            { "Articulos", Articulos },
            { "IdEmpresa", IdEmpresa },
            {
                "AddNewDetalleLote",
                EventCallback.Factory.Create<DetalleDto>(this, AddNewDetalleLote)
            },
            { "DetalleParaVenta", _detalleParaVenta }
        };

        await DialogService.ShowAsync<AgregarDetalleVentaModal>("Ingrese los detalles",
            parameters, _options);
    }

    private decimal GetTotalVenta() =>
        _detalleParaVenta.Sum(d => d.PrecioVenta * d.Cantidad);


    private async Task AgregarComprobanteSiTieneConfig()
    {
        var comprobantes = await ComprobanteService.GetComprobantesAsync(IdEmpresa);
        var ultimoIdComprobante = comprobantes.Last().IdComprobante;
        var total = GetTotalVenta();
        var url =
            $"https://localhost:44352/detalleComprobantes/agergarDetalleComprobante/{ultimoIdComprobante}";
        var detalles = new List<DetalleComprobanteDto>();

        var detalleCaja = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa!.Cuenta1!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta1.Value),
            Glosa         = "Venta de mercaderias",
            MontoDebe     = total,
            MontoDebeAlt  = decimal.Zero,
            MontoHaber    = decimal.Zero,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };

        var detalleIt = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa!.Cuenta5!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta5.Value),
            Glosa         = "Venta de mercaderias",
            MontoDebe     = total * 0.03m,
            MontoDebeAlt  = decimal.Zero,
            MontoHaber    = decimal.Zero,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };
        var detalleDebitoFiscal = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa!.Cuenta3!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta3.Value),
            Glosa         = "Venta de mercaderias",
            MontoDebe     = decimal.Zero,
            MontoDebeAlt  = decimal.Zero,
            MontoHaber    = total * 0.13m,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };
        var detalleVentas = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa!.Cuenta6!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta6.Value),
            Glosa         = "Venta de mercaderias",
            MontoDebe     = decimal.Zero,
            MontoDebeAlt  = decimal.Zero,
            MontoHaber    = detalleCaja.MontoDebe - detalleDebitoFiscal.MontoHaber,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };
        var detalleItPorPagar = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa!.Cuenta7!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta7.Value),
            Glosa         = "Venta de mercaderias",
            MontoDebe     = decimal.Zero,
            MontoDebeAlt  = decimal.Zero,
            MontoHaber    = total * 0.03m,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };
        detalles.Add(detalleCaja);
        detalles.Add(detalleIt);
        detalles.Add(detalleVentas);
        detalles.Add(detalleDebitoFiscal);
        detalles.Add(detalleItPorPagar);


        foreach (var detalle in detalles)
        {
            var response = await HttpClient.PostAsJsonAsync(url, detalle);
            response.EnsureSuccessStatusCode();
        }

        Snackbar.Add("Se ha generado un comprobante", Severity.Info,
            options => { options.ShowTransitionDuration = 2; });
    }

    private async Task CreateComprobante()
    {
        var empresaMonedas =
            await EmpresaMonedaService.GetEmpresaMonedasActiveAsync(IdEmpresa);
        var tipoCambio = empresaMonedas.Last().Cambio!.Value;
        var url        = $"https://localhost:44352/agregarcomprobante/{IdEmpresa}";
        var comprobanteDto = new ComprobanteDto
        {
            Glosa           = "Venta de mercaderias",
            Fecha           = Fecha!.Value,
            Tc              = tipoCambio,
            TipoComprobante = TipoComprobante.Egreso,
            IdMoneda        = empresaMonedas.Last().IdMonedaPrincipal,
            IdEmpresa       = IdEmpresa,
            IdUsuario       = 1
        };

        var response = await HttpClient.PostAsJsonAsync(url, comprobanteDto);
        if (response.IsSuccessStatusCode)
        {
            await AgregarComprobanteSiTieneConfig();
        }
    }

    private async Task<string> SearchCuenta(int idCuenta)
    {
        var cuentas = await CuentaService.GetCuentasAsync(IdEmpresa);
        var nombreCuenta =
            cuentas.SingleOrDefault(c => c.IdCuenta == idCuenta);
        if (nombreCuenta is null)
        {
            return await Task.FromResult(string.Empty);
        }

        return await Task.FromResult($"{nombreCuenta.Codigo} - {nombreCuenta.Nombre}");
    }

    private async Task AgregarVenta()
    {
        if (_detalleParaVenta.Count == 0)
        {
            Snackbar.Add("Debe agregar al menos un articulo", Severity.Info);
            return;
        }

        if (Empresa.TieneIntegracion!.Value)
        {
            await CreateComprobante();
        }

        var          total   = GetTotalVenta();
        const string url     = "https://localhost:44321/notas/agregarNota";
        var          nroNota = GetNextNumeroNota();
        var nota = new NotaDto
        {
            NroNota       = nroNota,
            Fecha         = Fecha!.Value,
            Descripcion   = Descripcion!,
            Total         = total,
            IdEmpresa     = IdEmpresa,
            IdUsuario     = 1,
            IdComprobante = null,
            TipoNota      = TipoNota.Venta
        };
        var response = await HttpClient.PostAsJsonAsync(url, nota);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Nota agregada exitosamente", Severity.Success);
            await AgregarDetalleVenta();
            NavigationManager.NavigateTo(
                $"/anularNotaVenta/{IdEmpresa}/{Notas.Last().IdNota}");
        }
    }

    private async Task GetNotaVentas() =>
        Notas = (await NotaService.GetNotaVentasAsync(IdEmpresa))!;

    private async Task AgregarDetalleVenta()
    {
        await GetNotaVentas();
        var          ultimoIdNota = Notas.Last().IdNota;
        const string url = "https://localhost:44321/detalleVentas/agregarDetalleVenta";
        try
        {
            foreach (var detalle in _detalleParaVenta)
            {
                detalle.IdNota = ultimoIdNota;
                var response = await HttpClient.PostAsJsonAsync(url, detalle);
                response.EnsureSuccessStatusCode();
            }

            Snackbar.Add("Detalles guardados exitosamente", Severity.Success);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while posting details: {e.Message}");
            Snackbar.Add("Error al guardar los detalles", Severity.Error);
        }
    }
}