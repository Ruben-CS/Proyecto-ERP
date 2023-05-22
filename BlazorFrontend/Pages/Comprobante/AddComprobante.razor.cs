using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;


namespace BlazorFrontend.Pages.Comprobante;

public partial class AddComprobante
{
    private bool _success;

    private MudForm? _form;

    #region DataForComprobante

    private DateTime? Fecha { get; set; } = DateTime.Today;

    private string? Glosa                 { get; set; } = string.Empty;
    private string? SelectedEmpresaMoneda { get; set; }

    private string? SerieString { get; set; } = string.Empty;

    private decimal? TipoDeCambio { get; set; }

    private string TipoDeCambioString
    {
        get => TipoDeCambio?.ToString("F2") ?? string.Empty;
        set
        {
            if (decimal.TryParse(value, out var result))
            {
                TipoDeCambio = result;
            }
            else
            {
                TipoDeCambio = null;
            }
        }
    }

    private string? SelectedTipoComprobante { get; set; }

    #endregion

    #region Fields

    private List<EmpresaMonedaDto>? EmpresaMonedas { get; set; } = new();

    private List<string>     ComprobanteTypes   { get; set; } = new();
    private List<MonedaDto?> MonedasDeLaEmpresa { get; set; } = new();

    private List<ComprobanteDto> Comprobantes { get; set; } = new();

    public List<GestionDto> Gestiones { get; set; } = new();

    public List<PeriodoDto> Periodos { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Datagrid Data

    private readonly ObservableCollection<DetalleComprobanteDto> _detalles = new();

    #endregion

    #region Validations

    private bool HasMoreThanTwoDetalles() => _detalles.Count < 2;

    private bool SumDebeAndHaberIsEqual() => TotalDebe.Equals(TotalHaber);

    private async Task<bool> ValidateFechaComprobante()
    {
        var gestionesActivas = Gestiones.Where(g => g.Estado == EstadosGestion.Abierto).ToList();
        var periodoActivoAlt = new List<PeriodoDto>();

        var periodoActivo = await PeriodoService.GetPeriodosAsync(gestionesActivas.First().IdGestion);
        if (gestionesActivas.Count != 1)
        {
            periodoActivoAlt = await PeriodoService.GetPeriodosAsync(gestionesActivas.Last().IdGestion);
        }

        if (periodoActivo.Any(p => Fecha!.Value     >= p.FechaInicio  && Fecha!.Value <= p.FechaFin) ||
            periodoActivoAlt.Any(pa => Fecha!.Value >= pa.FechaInicio && Fecha!.Value <= pa.FechaFin))
        {
            return await Task.FromResult(true);
        }

        return false;
    }

    private async Task InitializeGestion() => Gestiones = await GestionServices.GetGestionAsync(IdEmpresa);

    #endregion

    private decimal TotalDebe => _detalles.Sum(x => x.MontoDebe);

    private decimal TotalHaber => _detalles.Sum(x => x.MontoHaber);

    private async Task GoBack() => await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));


    private async Task RefreshComprobanteList() =>
        Comprobantes = await ComprobanteService.GetComprobantesAsync(IdEmpresa);

    protected override async Task OnInitializedAsync()
    {
        EmpresaMonedas     = await EmpresaMonedaService.GetEmpresasMonedaAsync(IdEmpresa);
        MonedasDeLaEmpresa = (await MonedaService.GetMonedasAsync())!;
        Comprobantes       = await ComprobanteService.GetComprobantesAsync(IdEmpresa);
        ComprobanteTypes   = Enum.GetNames(typeof(TipoComprobante)).ToList();
        await InitializeGestion();
        var nextSerie = GetNextSerie(IdEmpresa).Result;
        SerieString = nextSerie.ToString();
        //TODO Optimize this code
        var idMonedaPrincipal   = EmpresaMonedas!.First().IdMonedaPrincipal;
        var idMonedaAlternativa = EmpresaMonedas.Last().IdMonedaAlternativa;
        TipoDeCambio = await SetTipoCambio(idMonedaAlternativa);
        MonedasDeLaEmpresa = MonedasDeLaEmpresa
                             .Where(m => m.IdMoneda == idMonedaPrincipal ||
                                         m.IdMoneda == idMonedaAlternativa)
                             .ToList();

        #region Snackbar Config
        Snackbar.Configuration.PositionClass        = Defaults.Classes.Position.BottomLeft;
        Snackbar.Configuration.ClearAfterNavigation = true;
        Snackbar.Configuration.ShowCloseIcon        = false;
        Snackbar.Configuration.PreventDuplicates    = true;
        #endregion

        await InvokeAsync(StateHasChanged);
    }

    private void AddNewDetalleComprobante(DetalleComprobanteDto detalleComprobanteDto)
    {
        _detalles.Add(detalleComprobanteDto);
    }

    private async Task<int> GetNextSerie(int idEmpresa)
    {
        var maxSerie = Comprobantes
                       .Where(e => e.IdEmpresa == idEmpresa).Max(e => e.Serie)!;

        return await Task.FromResult((maxSerie ?? 0) + 1);
    }

    private async Task<decimal?> SetTipoCambio(int? idMonedaAlternativa)
    {
        if (idMonedaAlternativa is null)
        {
            return default;
        }

        return await Task.FromResult(EmpresaMonedas.Single(em => em.IdMonedaAlternativa == idMonedaAlternativa
                                                                 && em.Estado == EstadoEmpresaMoneda.Abierto).Cambio);
    }

    private async Task OpenAgregarDetalleModal()
    {
        var options = new DialogOptions
        {
            MaxWidth             = MaxWidth.Large,
            DisableBackdropClick = true,
            Position             = DialogPosition.TopCenter
        };
        var parameters = new DialogParameters
        {
            { "IdEmpresa", IdEmpresa },
            { "Glosa", Glosa },
            {
                "AddNewDetalleComprobante",
                EventCallback.Factory.Create<DetalleComprobanteDto>(this, AddNewDetalleComprobante)
            },
            { "Detalles", _detalles }
        };
        await DialogService.ShowAsync<DetalleComprobanteModal>("Ingrese los detalles del comprobante", parameters,
            options);
    }

    private async Task AgregarComprobante()
    {
        var url = $"https://localhost:44352/agregarcomprobante/{IdEmpresa}";

        if (!Enum.TryParse(typeof(TipoComprobante), SelectedTipoComprobante, out var parsedEnum))
        {
            throw new Exception("Unable to parse SelectedTipoComprobante to TipoComprobante enum");
        }

        var tipoComprobante = (TipoComprobante)parsedEnum;
        var idMoneda        = MonedasDeLaEmpresa.First(m => m!.Nombre == SelectedEmpresaMoneda)!.IdMoneda;
        var comprobanteDto = new ComprobanteDto
        {
            Glosa           = Glosa!,
            Fecha           = Fecha!.Value,
            Tc              = TipoDeCambio,
            TipoComprobante = tipoComprobante,
            IdMoneda        = idMoneda,
            IdEmpresa       = IdEmpresa,
            IdUsuario       = 1
        };
        if (HasMoreThanTwoDetalles())
        {
            Snackbar.Add("Debe agregar al menos dos detalles", Severity.Error);
            return;
        }

        if (!SumDebeAndHaberIsEqual())
        {
            Snackbar.Add("El total del debe y el haber deben ser iguales", Severity.Error);
            return;
        }

        if (!await ValidateFechaComprobante())
        {
            Snackbar.Add("La fecha del comprobante no esta dentro de un periodo activo", Severity.Error);
            return;
        }

        var response = await HttpClient.PostAsJsonAsync(url, comprobanteDto);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Comprobante agregado exitosamente", Severity.Success);
            await OnSerieChanged();
        }
    }

    private async Task OnSerieChanged()
    {
        await RefreshComprobanteList();
        SerieString = GetNextSerie(IdEmpresa).ToString();
    }
}