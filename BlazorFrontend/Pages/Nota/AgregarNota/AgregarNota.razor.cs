using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarNota
{
    private bool     _success;
    private MudForm? _form;
    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    public List<ArticuloDto> Articulos { get; set; } = new();
    private async Task GoBack() =>
        await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
    }

    private void GetNextNumeroNota()
    {

    }
}