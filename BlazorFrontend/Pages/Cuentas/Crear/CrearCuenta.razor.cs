using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Cuentas.Crear;

public partial class CrearCuenta
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

    private async Task CreateCuenta()
    {
        const string url = "https://localhost:44378/cuentas/agregarcuenta";

        var cuentaDto = new CuentaDto
        {
            Nombre        = CuentaDto.Nombre,
            IdCuentaPadre = SelectedValue?.IdCuenta,
            IdEmpresa     = IdEmpresa
        };
        if (await ValidateName(cuentaDto))
        {
            Snackbar.Add("Este nombre ya existe", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, cuentaDto);
            Snackbar.Add("Cuenta creada exitosamente", Severity.Success);
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