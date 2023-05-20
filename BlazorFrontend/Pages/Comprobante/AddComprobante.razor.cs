using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;


namespace BlazorFrontend.Pages.Comprobante;

public partial class AddComprobante
{
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

    #endregion

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Datagrid Data

    private readonly ObservableCollection<DetalleComprobanteDto> _detalles = new();

    #endregion


    protected override async Task OnInitializedAsync()
    {
        EmpresaMonedas     = await EmpresaMonedaService.GetEmpresasMonedaAsync(IdEmpresa);
        MonedasDeLaEmpresa = (await MonedaService.GetMonedasAsync())!;
        Comprobantes       = await ComprobanteService.GetComprobantesAsync(IdEmpresa);
        ComprobanteTypes   = Enum.GetNames(typeof(TipoComprobante)).ToList();

        var nextSerie = GetNextSerie(IdEmpresa);
        SerieString = nextSerie.ToString();
        //TODO Optimize this code
        var idMonedaPrincipal   = EmpresaMonedas!.First().IdMonedaPrincipal;
        var idMonedaAlternativa = EmpresaMonedas.Last().IdMonedaAlternativa;
        TipoDeCambio = await SetTipoCambio(idMonedaAlternativa);
        MonedasDeLaEmpresa = MonedasDeLaEmpresa
                             .Where(m => m.IdMoneda == idMonedaPrincipal ||
                                         m.IdMoneda == idMonedaAlternativa)
                             .ToList();

        await InvokeAsync(StateHasChanged);
    }

    private void AddNewDetalleComprobante(DetalleComprobanteDto detalleComprobanteDto)
    {
        _detalles.Add(detalleComprobanteDto);
    }

    private int GetNextSerie(int idEmpresa)
    {
        var maxSerie = Comprobantes
                       .Where(e => e.Estado   == EstadoComprobante.Abierto &&
                                   e.IdMoneda == idEmpresa).Max(e => e.Serie)!;

        return (maxSerie ?? 0) + 1;
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
            {"_detalles", _detalles}
        };
        await DialogService.ShowAsync<DetalleComprobanteModal>("Ingrese los detalles del comprobante", parameters, options);
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

        var response = await HttpClient.PostAsJsonAsync(url, comprobanteDto);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Comprobante agregado exitosamente", Severity.Success);
        }
    }
}