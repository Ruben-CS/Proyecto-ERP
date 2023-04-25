using BlazorFrontend.Pages.Cuentas.Crear;
using BlazorFrontend.Pages.Cuentas.Editar;
using BlazorFrontend.Pages.Cuentas.Eliminar;
using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Cuentas;

public partial class CuentasOverview
{
    private bool IsExpanded { get; set; }

    private Dictionary<TreeItemData, HashSet<TreeItemData>> RootItems { get; set; }

    private TreeItemData SelectedValue { get; set; }

    private bool _open;

    private bool _folderOneExpanded;

    private List<CuentaDto> _cuentas = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    private HashSet<TreeItemData> TreeItems { get; set; } = new();

    private bool IsSelected() => SelectedValue is null;

    private void ToggleDrawer() => _open = !_open;

    private void CambiarEmpresa() => NavigationManager.NavigateTo("/inicio");


    public class TreeItemData
    {
        public int     IdCuenta { get; set; }
        public string? Codigo   { get; set; }
        public string  Nombre   { get; set; }

        public string? TipoCuenta { get; set; }

        public int?                  IdCuentaPadre { get; set; }
        public HashSet<TreeItemData> CuentasHijas  { get; set; }

        public TreeItemData(CuentaDto cuenta)
        {
            IdCuenta      = cuenta.IdCuenta;
            Codigo        = cuenta.Codigo;
            Nombre        = cuenta.Nombre;
            IdCuentaPadre = cuenta.IdCuentaPadre;
            TipoCuenta    = cuenta.TipoCuenta;
            CuentasHijas  = new HashSet<TreeItemData>();
        }
    }

    private async Task<bool> HasChildren(TreeItemData selectedValue) =>
        await Task.FromResult(_cuentas.Any(cuenta =>
            cuenta.IdCuentaPadre == selectedValue.IdCuenta));

    private static TreeItemData CreateTree(TreeItemData           treeItemData,
                                           IEnumerable<CuentaDto> allCuentas)
    {
        var childCuentas = allCuentas.Where(c => c.IdCuentaPadre == treeItemData.IdCuenta)
                                     .ToList();

        foreach (var childCuenta in childCuentas)
        {
            treeItemData.CuentasHijas.Add(CreateTree(new TreeItemData(childCuenta),
                allCuentas));
        }

        return treeItemData;
    }

    private static Dictionary<TreeItemData, HashSet<TreeItemData>> CreateRootItems(
        List<CuentaDto> cuentas)
    {
        var rootItems = new Dictionary<TreeItemData, HashSet<TreeItemData>>();

        foreach (var cuenta in cuentas)
        {
            if (cuenta.IdCuentaPadre is not null) continue;
            var rootItem = new TreeItemData(cuenta);
            var children = CreateTree(rootItem, cuentas);
            rootItems.Add(rootItem, new HashSet<TreeItemData> { children });
        }

        return rootItems;
    }


    private static HashSet<TreeItemData> BuildTreeItems(
        IReadOnlyCollection<CuentaDto> cuentas)
    {
        var rootCuentas = cuentas.Where(c => c.IdCuentaPadre == null).ToList();
        var treeItems = new HashSet<TreeItemData>(rootCuentas.Select(c =>
            new TreeItemData(c)
            {
                CuentasHijas = BuildTreeItemChildren(c, cuentas)
            }));

        return treeItems;
    }

    private static HashSet<TreeItemData> BuildTreeItemChildren(
        CuentaDto parentCuenta, IEnumerable<CuentaDto> cuentas)
    {
        var childCuentas = cuentas.Where(c => c.IdCuentaPadre == parentCuenta.IdCuenta)
                                  .ToList();
        var treeItemChildren = new HashSet<TreeItemData>(childCuentas.Select(c =>
            new TreeItemData(c) { CuentasHijas = BuildTreeItemChildren(c, cuentas) }));

        return treeItemChildren;
    }

    private async Task LoadCuentas()
    {
        var cuentas = await CuentaService.GetCuentasAsync(IdEmpresa);

        RootItems = cuentas.Any()
            ? CreateRootItems(cuentas)
            : new Dictionary<TreeItemData, HashSet<TreeItemData>>();
    }

    protected override async Task OnInitializedAsync()
    {
        try
        {
            var uri      = new Uri(NavigationManager.Uri);
            var segments = uri.Segments;
            var idValue  = segments[^1];
            if (!string.IsNullOrEmpty(idValue) && int.TryParse(idValue, out _))
            {
                IdEmpresa = int.Parse(idValue);
                _cuentas  = await CuentaService.GetCuentasAsync(IdEmpresa);
                TreeItems = BuildTreeItems(_cuentas);
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

    private async Task ShowCrearCuenta()
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
                EventCallback.Factory.Create<CuentaDto>(this, OnTreeViewChange)
            }
        };

        await DialogService.ShowAsync<CrearCuenta>
            ("Escriba el nombre de la cuenta", parameters, options);
    }

    private async Task ShowEditarCuenta()
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
                EventCallback.Factory.Create<CuentaDto>(this, OnTreeViewChange)
            }
        };

        await DialogService.ShowAsync<EditarCuenta>
            ("Edite el nombre", parameters, options);
    }

    private async Task ShowEliminarCuenta()
    {
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
                EventCallback.Factory.Create<CuentaDto>(this, OnTreeViewChange)
            }
        };
        if (await HasChildren(SelectedValue))
        {
            Snackbar.Add("No puede eliminar una cuenta no vacia", Severity.Error);
        }
        else
        {
            await DialogService.ShowAsync<EliminarCuenta>
                ("Esta seguro?", parameters, options);
        }
    }


    private void NavigateToGestiones()
    {
        if (IdEmpresa is 0) return;
        var uri = $"/gestion/overview/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }

    private async Task OnTreeViewChange(CuentaDto cuentaDto)
    {
        _cuentas  = await CuentaService.GetCuentasAsync(IdEmpresa);
        TreeItems = BuildTreeItems(_cuentas);
        await LoadCuentas();
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }
}