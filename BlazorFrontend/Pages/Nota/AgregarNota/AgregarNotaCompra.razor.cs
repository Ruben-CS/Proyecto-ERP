using System.Collections.ObjectModel;
using System.Text;
using Microsoft.AspNetCore.Components;
using Microsoft.IdentityModel.Tokens;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarNotaCompra
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

    #region Other Properties

    private EmpresaDto Empresa { get; set; } = new();

    private List<ComprobanteDto> Comprobantes { get; set; } = new();

    private List<CuentaDto>? Cuentas { get; set; } = new();

    #endregion

    private List<NotaDto> Notas { get; set; } = new();

    private List<ArticuloDto> Articulos { get; set; } = new();

    private readonly ObservableCollection<LoteDto> _detalleParaLote = new();

    public bool IsLoading { get; set; }

    private async Task GoBack() =>
        await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));

    private void AddNewDetalleLote(LoteDto lote) => _detalleParaLote.Add(lote);


    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        await GetNotaCompras();
        var nextNro = GetNextNumeroNota();
        NroNota = nextNro.ToString();
        Empresa = (await EmpresaService.GetEmpresaByIdAsync(IdEmpresa))!;
        Cuentas = await CuentaService.GetCuentasDetalle(IdEmpresa);
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
            { "FechaIngreso", Fecha },
            { "Articulos", Articulos },
            { "IdEmpresa", IdEmpresa },
            {
                "AddNewDetalleLote",
                EventCallback.Factory.Create<LoteDto>(this, AddNewDetalleLote)
            },
            { "_detalleParaLote", _detalleParaLote }
        };

        await DialogService.ShowAsync<AgregarDetalleModal>("Ingrese los detalles",
            parameters, _options);
    }

    private async Task RefreshComprobanteList() =>
        Comprobantes = await ComprobanteService.GetComprobantesAsync(IdEmpresa);

    private async Task AgregarComprobanteSiTieneConfig()
    {
        await RefreshComprobanteList();
        var ultimoIdComprobante = Comprobantes.Last().IdComprobante;
        var total               = _detalleParaLote.Sum(d => d.PrecioCompra * d.Cantidad);
        var url =
            $"https://localhost:44352/detalleComprobantes/agergarDetalleComprobante/{ultimoIdComprobante}";
        var detalles = new List<DetalleComprobanteDto>();
        var detalleCaja = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa.Cuenta1!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta1.Value),
            Glosa         = "Compra de mercaderias",
            MontoDebe     = decimal.Zero,
            MontoHaber    = total,
            MontoDebeAlt  = decimal.Zero,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };

        var detalleCreditoFiscal = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa.Cuenta2!.Value,
            NombreCuenta  = await SearchCuenta(Empresa.Cuenta2.Value),
            Glosa         = "Compra de mercaderias",
            MontoDebe     = total - (total * (decimal)0.13),
            MontoHaber    = decimal.Zero,
            MontoDebeAlt  = decimal.Zero,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };

        var detalleCompras = new DetalleComprobanteDto
        {
            IdCuenta      = Empresa.Cuenta3!.Value,
            NombreCuenta  =  await SearchCuenta(Empresa.Cuenta3.Value),
            Glosa         = "Compra de mercaderias",
            MontoDebe     = detalleCaja.MontoHaber - detalleCreditoFiscal.MontoDebe,
            MontoHaber    = decimal.Zero,
            MontoDebeAlt  = decimal.Zero,
            MontoHaberAlt = decimal.Zero,
            IdComprobante = ultimoIdComprobante,
            IdUsuario     = 1
        };
        detalles.Add(detalleCaja);
        detalles.Add(detalleCreditoFiscal);
        detalles.Add(detalleCompras);

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
        IsLoading = true;
        var empresaMonedas =
            await EmpresaMonedaService.GetEmpresaMonedasActiveAsync(IdEmpresa);
        var tipoCambio = empresaMonedas.Last().Cambio!.Value;
        var url        = $"https://localhost:44352/agregarcomprobante/{IdEmpresa}";
        var comprobanteDto = new ComprobanteDto
        {
            Glosa           = "Compra de mercaderias",
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
            IsLoading = false;
        }
    }

    private async Task AgregarCompra()
    {
        if (_detalleParaLote.Count == 0)
        {
            Snackbar.Add("Debe agregar al menos un articulo", Severity.Info);
            return;
        }

        if (Empresa.TieneIntegracion!.Value)
        {
            await CreateComprobante();
        }

        var          total   = _detalleParaLote.Sum(d => d.PrecioCompra * d.Cantidad);
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
            TipoNota      = TipoNota.Compra
        };
        var response = await HttpClient.PostAsJsonAsync(url, nota);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Nota agregada exitosamente", Severity.Success);
            await AgregarLote();
            NavigationManager.NavigateTo(
                $"/anularNotaCompra/{IdEmpresa}/{Notas.Last().IdNota}");
        }
    }

    private async Task GetNotaCompras() =>
        Notas = (await NotaService.GetNotaComprasAsync(IdEmpresa))!;

    private async Task AgregarLote()
    {
        await GetNotaCompras();
        var ultimoIdNota = Notas.Last().IdNota;
        var url          = $"https://localhost:44321/lotes/agregarLote/{ultimoIdNota}";
        var emptyContent = new StringContent("", Encoding.UTF8, "application/json");
        try
        {
            foreach (var lote in _detalleParaLote)
            {
                lote.IdNota = ultimoIdNota;
                var nroLote = await CheckIfLoteHasExistingArticulo(lote.IdArticulo);
                lote.NroLote = nroLote;
                var urlEditarCantidadArticulo =
                    $"https://localhost:44321/articulos/editarArticuloCantidad/{lote.IdArticulo}/{lote.Cantidad}";
                var response = await HttpClient.PostAsJsonAsync(url, lote);
                var responseEditar =
                    await HttpClient.PutAsJsonAsync(urlEditarCantidadArticulo,
                        emptyContent);
                response.EnsureSuccessStatusCode();
                responseEditar.EnsureSuccessStatusCode();
            }

            Snackbar.Add("Detalles guardados exitosamente", Severity.Success);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while posting details: {e.Message}");
            Snackbar.Add("Error al guardar los detalles", Severity.Error);
        }
    }

    private async Task<string> SearchCuenta(int idCuenta)
    {
        var nombreCuenta =
            Cuentas!.SingleOrDefault(c => c.IdCuenta == idCuenta);
        if (nombreCuenta is null)
        {
            return string.Empty;
        }

        return $"{nombreCuenta.Codigo} - {nombreCuenta.Nombre}";
    }

    private async Task<int?> CheckIfLoteHasExistingArticulo(int idArticulo)
    {
        var lotePorArticulo = await LoteService.GetLotesPerArticleIdAsync(idArticulo);
        if (lotePorArticulo.IsNullOrEmpty())
            return 1;
        return lotePorArticulo!.Max(n => n.NroLote) + 1;
    }
}