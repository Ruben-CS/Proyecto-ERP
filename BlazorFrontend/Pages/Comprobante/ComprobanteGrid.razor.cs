using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;

namespace BlazorFrontend.Pages.Comprobante;

public partial class ComprobanteGrid
{
    #region DataForComprobante

    private ComprobanteDto ComprobanteDto { get; set; } = new();
    public  DateTime       Fecha          { get; set; } = DateTime.Today;

    private List<string>? MonedasName           { get; set; }
    private string?       SelectedEmpresaMoneda { get; set; }

    #endregion

    #region Fields

    private IEnumerable<EmpresaMonedaDto>? MonedasPerEmpresa { get; set; }

    #endregion

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        await GetPrimaryAndAltMonedaAsync(IdEmpresa);
        await InvokeAsync(StateHasChanged);
    }

    private async Task GetPrimaryAndAltMonedaAsync(int idEmpresa)
    {
        var empresMonedas = await EmpresaMonedaService.GetEmpresasMonedaAsync(idEmpresa);
        MonedasPerEmpresa = empresMonedas.Where(em => em.Cambio == null && em.Estado == EstadoEmpresaMoneda.Activo)
                                         .ToList();
    }

}