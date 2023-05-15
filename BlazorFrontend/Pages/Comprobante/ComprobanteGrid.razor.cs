using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;

namespace BlazorFrontend.Pages.Comprobante;

public partial class ComprobanteGrid
{
    #region DataForComprobante

    private ComprobanteDto ComprobanteDto { get; set; } = new();
    public  DateTime?       Fecha          { get; set; } = DateTime.Today;

    private List<string>? MonedasName           { get; set; }
    private string?       SelectedEmpresaMoneda { get; set; }

    #endregion

    #region Fields

    private List<EmpresaMonedaDto>? EmpresaMonedas { get; set; } = new();

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

        //TODO Optimize this code
        var idMonedaPrincipal   = EmpresaMonedas!.First().IdMonedaPrincipal;
        var idMonedaAlternativa = EmpresaMonedas.Last().IdMonedaAlternativa;
        MonedasDeLaEmpresa = MonedasDeLaEmpresa
                             .Where(m => m.IdMoneda == idMonedaPrincipal ||
                                         m.IdMoneda == idMonedaAlternativa)
                             .ToList();

        await InvokeAsync(StateHasChanged);
    }
}