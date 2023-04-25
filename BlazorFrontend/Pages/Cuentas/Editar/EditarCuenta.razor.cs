using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.Cuentas.Editar;

public partial class EditarCuenta
{
    public CuentaDto CuentaDto { get; set; } = new();

    private List<CuentaDto> _cuentas = new();

    [Parameter]
    public CuentasOverview.TreeItemData? SelectedValue { get; set; }

    [Parameter]
    public EventCallback<CuentaDto> OnTreeViewChange { get; set; }

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public int IdEmpresa { get; set; }

    private void Cancel() => MudDialog!.Cancel();

    protected override async Task OnInitializedAsync()
    {
        _cuentas = await CuentaService.GetCuentasAsync(IdEmpresa);
    }

    private async Task EditCuenta()
    {
        var url =
            $"https://localhost:44378/cuentas/actualizarcuenta/{SelectedValue?.IdCuenta}";

        var cuentaDto = new CuentaDto
        {
            IdCuenta = SelectedValue.IdCuenta,
            Nombre = SelectedValue.Nombre,
            TipoCuenta = SelectedValue.TipoCuenta,
            Codigo = SelectedValue.Codigo
        };
        if (await ValidateName(cuentaDto))
        {
            Snackbar.Add("Este nombre ya existe", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PutAsJsonAsync(url, cuentaDto);
            Snackbar.Add("Cuenta editada exitosamente", Severity.Success);
            await OnTreeViewChange.InvokeAsync(cuentaDto);
            MudDialog!.Close(DialogResult.Ok(response));
        }
    }

    private async Task<bool> ValidateName(CuentaDto cuentaDto)
    {
        return await Task.FromResult(_cuentas.Any(c =>
            c.Nombre    == cuentaDto.Nombre &&
            c.IdEmpresa == cuentaDto.IdEmpresa));
    }
}