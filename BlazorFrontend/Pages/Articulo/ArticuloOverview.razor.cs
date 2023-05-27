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

    private string SearchString { get; set; } = string.Empty;

    public  bool IsLoading                    { get; set; }
    private bool FilterFunc1(ArticuloDto dto) => FilterFunc2(dto, SearchString);

    private readonly Dictionary<int, List<string>> _articuloCategorias = new();

    private List<int> _idArticulos = new();

    private static bool FilterFunc2(ArticuloDto dto, string searchString)
    {
        if (string.IsNullOrWhiteSpace(searchString))
            return true;
        if (dto.Nombre!.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
            return true;
        if (dto.Nombre!.Contains(searchString, StringComparison.CurrentCultureIgnoreCase))
            return true;
        if ($"{dto.Nombre} {dto.Descripcion}".Contains(searchString))
            return true;
        return false;
    }

    private async Task GetCategoriesForArticles(List<int> articleIds)
    {
        _articuloCategorias.Clear();

        foreach (var articuloId in articleIds)
        {
            var articuloCategorias =
                await ArticuloCategoriaService.GetArticuloCategoriasAsync(articuloId);
            if (articuloCategorias.Count > 0)
            {
                var nombresCategorias = articuloCategorias
                                        .Select(ac => ac.NombreCategoria).ToList();
                _articuloCategorias.Add(articuloId, nombresCategorias);
            }
        }

        StateHasChanged();
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            IsLoading = true;
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa  = int.Parse(idValue);
                Articulos  = await ArticuloService.GetArticulosAsync(IdEmpresa);
                Categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
                await InvokeAsync(StateHasChanged);
                _idArticulos = Articulos.Select(a => a.IdArticulo).ToList();
                await GetCategoriesForArticles(_idArticulos);
                IsLoading = false;
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
            MaxWidth             = MaxWidth.Small,
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
            {
                "OnArticuloAdded",
                EventCallback.Factory.Create<ArticuloDto>(this, OnArticuloAdded)
            }
        };

        await DialogService.ShowAsync<CrearArticulo>
            ("Rellene los datos del articulo", parameters, options);
    }

    private async Task OnArticuloAdded(ArticuloDto dto)
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        await InvokeAsync(StateHasChanged);
    }
}