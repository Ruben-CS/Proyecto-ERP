using BlazorFrontend.Pages.Categoria.Crear;
using BlazorFrontend.Pages.Categoria.Editar;
using BlazorFrontend.Pages.Categoria.Eliminar;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Categoria;

public partial class CategoriaOverview
{
    [Parameter]
    public int IdEmpresa { get; set; }

    private TreeItemDataCategoria? SelectedValue { get; set; }

    private List<CategoriaDto> _categorias = new();

    private HashSet<TreeItemDataCategoria> TreeItems { get; set; } = new();

    private bool _folderOneExpanded;

    private bool IsLoading { get; set; }


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
                IdEmpresa   = int.Parse(idValue);
                _categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
                TreeItems   = BuildTreeItems(_categorias);
                await LoadCuentas();
                await InvokeAsync(StateHasChanged);
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

    private Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>> RootItems
    {
        get;
        set;
    } = null!;

    private static TreeItemDataCategoria CreateTree(TreeItemDataCategoria treeItemData)
    {
        return treeItemData;
    }

    private static Dictionary<TreeItemDataCategoria, HashSet<TreeItemDataCategoria>>
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

    private async Task ShowEliminarCategoria()
    {
        if (IsSelectedEmpty())
        {
            return;
        }

        var options = new DialogOptions
        {
            CloseOnEscapeKey     = true,
            MaxWidth             = MaxWidth.ExtraSmall,
            FullWidth            = true,
            DisableBackdropClick = true
        };
        var parameters = new DialogParameters
        {
            {
                "SelectedValue", SelectedValue
            },
            {
                "OnTreeViewChange",
                EventCallback.Factory.Create<CategoriaDto>(this, OnTreeViewChange)
            }
        };
        if (await HasChildren(SelectedValue))
        {
            Snackbar.Add("No puede eliminar una categoria con hijos", Severity.Error);
        }
        else
        {
            await DialogService.ShowAsync<EliminarCategoria>
                ("Esta seguro?", parameters, options);
        }
    }

    private async Task ShowEditarCategoria()
    {
        if (IsSelectedEmpty())
        {
            return;
        }

        var options = new DialogOptions
        {
            CloseOnEscapeKey     = true,
            MaxWidth             = MaxWidth.ExtraSmall,
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
        await DialogService.ShowAsync<EditarCategoria>
            ("Edite los campos", parameters, options);
    }

    private async Task<bool> HasChildren(TreeItemDataCategoria selectedValue) =>
        await Task.FromResult(_categorias.Any(cuenta =>
            cuenta.IdCategoriaPadre == selectedValue.IdCategoria));

    private bool IsSelectedEmpty()
    {
        if (SelectedValue is null)
        {
            Snackbar.Add("Seleccione una categoria primero", Severity.Info);
            return true;
        }

        return false;
    }


    private async Task OnTreeViewChange(CategoriaDto cuentaDto)
    {
        _categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
        TreeItems   = BuildTreeItems(_categorias);
        await LoadCuentas();
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }
}