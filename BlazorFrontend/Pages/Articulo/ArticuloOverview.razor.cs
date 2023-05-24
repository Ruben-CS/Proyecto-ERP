using BlazorFrontend.Pages.Articulo.Crear;
using BlazorFrontend.Pages.Categoria.Crear;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Articulo;

public partial class ArticuloOverview
{
    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    private List<ArticuloDto> Articulos { get; set; } = new();

    private List<CategoriaDto> Categorias { get; set; } = new();

    #endregion


    protected override async Task OnInitializedAsync()
    {

        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa  = int.Parse(idValue);
                Articulos  = await ArticuloService.GetArticulosAsync(IdEmpresa);
                Categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
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

    private async Task ShowCrearArticulo()
    {

        var options = new DialogOptions
        {
            CloseOnEscapeKey     = true,
            MaxWidth             = MaxWidth.Medium,
            FullWidth            = true,
            DisableBackdropClick = true
        };
        var parameters = new DialogParameters
        {
            {
                "CategoriaDtos", Categorias
            },
            {
                "IdEmpresa", IdEmpresa
            },
        };

        await DialogService.ShowAsync<CrearArticulo>
            ("Rellene los datos del articulo", parameters, options);
    }
}