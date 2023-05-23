using BlazorFrontend.Pages.Categoria.Crear;
using BlazorFrontend.Pages.Cuentas.Crear;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;
using Services.Cuenta;

namespace BlazorFrontend.Pages.Categoria;

public partial class CategoriaOverview
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private TreeItemDataCategoria? SelectedValue { get; set; }

    private List<CategoriaDto> _categorias = new();

    private HashSet<TreeItemDataCategoria> TreeItems { get; set; } = new();

    private bool _open;

    private bool _folderOneExpanded;


    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa   = int.Parse(idValue);
                _categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
                TreeItems   = BuildTreeItems(_categorias);
                await LoadCuentas();
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

    private Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>> RootItems
    {
        get;
        set;
    }

    private static TreeItemDataCategoria CreateTree(TreeItemDataCategoria treeItemData)
    {
        return treeItemData;
    }

    private Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>>
        CreateRootItems(
            List<CategoriaDto> categoriaDtos)
    {
        var rootItems =
            new Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>>();

        foreach (var cuenta in categoriaDtos)
        {
            if (cuenta.IdCategoriaPadre is not null) continue;
            var rootItem = new TreeItemDataCategoria(cuenta);
            var children = CreateTree(rootItem);
            rootItems.Add(rootItem, new HashSet<TreeItemDataCategoria> { children });
        }

        return rootItems;
    }


    private static HashSet<TreeItemDataCategoria> BuildTreeItems(
        IReadOnlyCollection<CategoriaDto> cuentas)
    {
        var rootCuentas = cuentas.Where(c => c.IdCategoriaPadre == null).ToList();
        var treeItems = new HashSet<TreeItemDataCategoria>(rootCuentas.Select(c =>
            new TreeItemDataCategoria(c)
            {
                CuentasHijas = BuildTreeItemChildren(c, cuentas)
            }));

        return treeItems;
    }

    private static HashSet<TreeItemDataCategoria> BuildTreeItemChildren(
        CategoriaDto parentCuenta, IEnumerable<CategoriaDto> cuentas)
    {
        var categoriaDtos = cuentas.ToList();

        var childCuentas = categoriaDtos
                           .Where(c => c.IdCategoriaPadre == parentCuenta.IdCategoria)
                           .ToList();
        var treeItemChildren = new HashSet<TreeItemDataCategoria>(childCuentas
            .Select(c =>
                new TreeItemDataCategoria(c)
                {
                    CuentasHijas = BuildTreeItemChildren(c, categoriaDtos)
                }));
        return treeItemChildren;
    }

    private async Task LoadCuentas()
    {
        var cuentas = await CategoriaService.GetCategoriasService(IdEmpresa);

        RootItems = cuentas.Any()
            ? CreateRootItems(cuentas)
            : new Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>>();
    }


    private async Task ShowCrearCategoria()
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
                "SelectedValue", SelectedValue
            },
            {
                "IdEmpresa", IdEmpresa
            },
            {
                "OnTreeViewChange",
                EventCallback.Factory.Create<CategoriaDto>(this, OnTreeViewChange)
            }
        };

        await DialogService.ShowAsync<CrearCategoria>
            ("Escriba el nombre de la cuenta", parameters, options);
    }

    private async Task OnTreeViewChange(CategoriaDto cuentaDto)
    {
        _categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
        TreeItems   = BuildTreeItems(_categorias);
        await LoadCuentas();
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }

}