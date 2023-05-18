using Microsoft.AspNetCore.Components;
using MudBlazor;

namespace BlazorFrontend.Pages.Comprobante;

public partial class DetalleComprobante
{
    [CascadingParameter]
    private MudDialogInstance MudDialog { get; set; } = null!;

    [Parameter]
    public int IdEmpresa { get; set; }


    #region Fields

    [Parameter]
    public string Glosa { get; set; }

    public decimal? Debe { get; set; }

    public decimal? Haber { get; set; }

    #endregion

    void Submit() => MudDialog.Close(DialogResult.Ok(true));

    void Cancel() => MudDialog.Cancel();
}