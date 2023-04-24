using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Cuentas.Crear;

public partial class CrearCuenta
{
    public CuentaDto CuentaDto { get; set; } = new();

    [Parameter]
    public CuentasOverview.TreeItemData? SelectedValue { get; set; }

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public int IdEmpresa { get; set; }

    private void Cancel() => MudDialog!.Cancel();

    private async Task CreateCuenta()
    {
        const string url = "https://localhost:44378/cuentas/agregarcuenta";

        var cuentaDto = new CuentaDto
        {
            Nombre        = CuentaDto.Nombre,
            TipoCuenta    = "Global",
            IdCuentaPadre = SelectedValue?.IdCuenta,
            IdEmpresa     = IdEmpresa
        };
        //todo add validations and event triggers
        var response = await HttpClient.PostAsJsonAsync(url, cuentaDto);
        Snackbar.Add("Cuenta creada exitosamente", Severity.Success);
        MudDialog!.Close(DialogResult.Ok(response));
    }
}