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
    MudDialogInstance? MudDialog { get; set; }

    private void Cancel() => MudDialog!.Cancel();

    private async Task CreateCuenta()
    {

    }
}