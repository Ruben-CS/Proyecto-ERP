using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class DetalleComprobante
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public int IdEmpresa { get; set; }

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

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
}