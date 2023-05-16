using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;

namespace BlazorFrontend.Pages.Comprobante;

public partial class ComprobanteGrid
{
    #region DataForComprobante

    private ComprobanteDto ComprobanteDto { get; set; } = new();
    private DateTime?      Fecha          { get; set; } = DateTime.Today;

    private List<string>? MonedasName           { get; set; }
    private string?       SelectedEmpresaMoneda { get; set; }

    private float? TipoDeCambio { get; set; }

    private string TipoDeCambioString
    {
        get => TipoDeCambio?.ToString() ?? string.Empty;
        set
        {
            if (float.TryParse(value, out var result))
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

    #endregion

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        EmpresaMonedas     = await EmpresaMonedaService.GetEmpresasMonedaAsync(IdEmpresa);
        MonedasDeLaEmpresa = (await MonedaService.GetMonedasAsync())!;
        ComprobanteTypes   = Enum.GetNames(typeof(TipoComprobante)).ToList();

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

    private async Task<float?> SetTipoCambio(int? idMonedaAlternativa)
    {
        if (idMonedaAlternativa is null)
        {
            return default;
        }

        return await Task.FromResult(EmpresaMonedas.Single(em => em.IdMonedaAlternativa == idMonedaAlternativa
                                                                 && em.Estado == EstadoEmpresaMoneda.Abierto).Cambio);
    }
}