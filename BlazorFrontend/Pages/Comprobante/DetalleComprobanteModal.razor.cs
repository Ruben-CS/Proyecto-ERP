using Microsoft.AspNetCore.Components;
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

    public List<CuentaDto> Cuentas { get; set; } = new();


    #region Fields

    [Parameter]
    public string? Glosa { get; set; }

    private decimal? Debe { get; set; }

    private decimal? Haber { get; set; }

    private string? SelectedCuenta { get; set; }

    #endregion

    protected override async Task OnInitializedAsync()
    {
        await base.OnInitializedAsync();
        Cuentas = await CuentaService.GetCuentasDetalle(IdEmpresa);
    }

    private async Task<IEnumerable<string>> SearchCuenta(string value)
    {
        IEnumerable<string> nombreCuentas = Cuentas.Select(c => $"{c.Codigo} - {c.Nombre}").ToList();
        if (string.IsNullOrEmpty(value))
        {
            return await Task.FromResult(nombreCuentas);
        }

        return nombreCuentas.Where(c => c.Contains(value, StringComparison.InvariantCultureIgnoreCase));
    }

    private async Task Submit()
    {
        //todo change this to get the id
        var detalleComprobante = new DetalleComprobanteDto()
        {
            NombreCuenta = SelectedCuenta,
            Glosa = Glosa!,
            MontoDebe = Debe!.Value,
            MontoHaber = Haber!.Value
        };

        await AddNewDetalleComprobante.InvokeAsync(detalleComprobante);
    }

    private void Cancel() => MudDialog.Cancel();
}