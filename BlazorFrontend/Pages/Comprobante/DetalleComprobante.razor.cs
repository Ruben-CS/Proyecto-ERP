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

    protected override Task OnInitializedAsync()
    {
        return base.OnInitializedAsync();
    }

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
}