using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota;

public partial class NotaVentaOverview
{
    private MudTable<NotaDto> _table;

    [Parameter]
    public int IdEmpresa { get; set; }

    private bool IsLoading { get; set; }

    private List<NotaDto>? Notas { get; set; } = new();

    private void PageChanged(int i) => _table.NavigateTo(i - 1);


    protected override async Task OnInitializedAsync()
    {
        IsLoading = true;
        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                await base.OnInitializedAsync();
                IdEmpresa = int.Parse(idValue);
                Notas     = await NotaService.GetNotaComprasAsync(IdEmpresa);
                IsLoading = false;
                await InvokeAsync(StateHasChanged);
            }
            else
            {
                throw new KeyNotFoundException(
                    "The 'id' parameter was not found in the query string.");
            }
        }
        catch (Exception ex)
        {
            Console.WriteLine(
                $"An error occurred while initializing the component: {ex}");
        }
    }

    private void NavigateToCrearNotaCompra()
    {
        var uri = $"/agregarNota/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }

    private void NavigateToDetalleNota(int idNota)
    {
        var uri = $" /anularNotaCompra/{IdEmpresa}/{idNota}";
        NavigationManager.NavigateTo(uri);
    }
}