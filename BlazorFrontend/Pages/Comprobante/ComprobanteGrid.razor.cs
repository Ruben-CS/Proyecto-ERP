using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;


namespace BlazorFrontend.Pages.Comprobante;

public partial class ComprobanteGrid
{
    #region DataForComprobante

    private ComprobanteDto ComprobanteDto { get; set; } = new();
    private DateTime?      Fecha          { get; set; } = DateTime.Today;

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

    public List<ComprobanteDto> Comprobantes { get; set; } = new();

    #endregion

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

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
            ClassBackground      = "modal-background"
        };
        var parameters = new DialogParameters
        {
            { "IdEmpresa", IdEmpresa }
        };
        await DialogService.ShowAsync<DetalleComprobante>(string.Empty, parameters, options);
    }
}