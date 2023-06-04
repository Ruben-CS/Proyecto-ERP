using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarNotaCompra
{
    private bool     _success;
    private MudForm? _form;
    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Nota Details

    private string? NroNota { get; set; }

    private string? Descripcion { get; set; }

    private DateTime? Fecha { get; set; } = DateTime.Today;
    #endregion

    private List<NotaDto>? _notas { get; set; } = new();

    public List<ArticuloDto> Articulos { get; set; } = new();
    private async Task GoBack() =>
        await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        _notas    = await NotaService.GetNotaComprasAsync(IdEmpresa);
        var nextNro = GetNextNumeroNota();
        NroNota = nextNro.ToString();
        await InvokeAsync(StateHasChanged);
    }

    private int GetNextNumeroNota()
    {
        var maxNroNota = _notas.Max(n => n.NroNota);
        return (maxNroNota ?? 0) + 1;
    }
}