using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Cuentas;

public partial class CuentasOverview
{
    private bool IsExpanded { get; set; }

    private bool _open;

    private bool _folderOneExpanded;

    private List<CuentaDto> _cuentas = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    private HashSet<TreeItemData> TreeItems { get; set; } = new();

    public class TreeItemData
    {
        public int                   IdCuenta     { get; set; }
        public string                Codigo       { get; set; }
        public string                Nombre       { get; set; }
        public HashSet<TreeItemData> CuentasHijas { get; set; }

        public TreeItemData(CuentaDto cuenta)
        {
            IdCuenta     = cuenta.IdCuenta;
            Codigo       = cuenta.Codigo;
            Nombre       = cuenta.Nombre;
            CuentasHijas = new HashSet<TreeItemData>();
        }
    }

    private static TreeItemData CreateTree(CuentaDto cuenta,
                                           IEnumerable<CuentaDto> allCuentas)
    {
        var treeItemData = new TreeItemData(cuenta);
        var childCuentas =
            allCuentas.Where(c => c.IdCuentaPadre == cuenta.IdCuenta).ToList();

        foreach (var childCuenta in childCuentas)
        {
            treeItemData.CuentasHijas.Add(CreateTree(childCuenta, allCuentas));
        }

        return treeItemData;
    }


    private HashSet<TreeItemData> BuildTreeItems(List<CuentaDto> cuentas)
    {
        // Find root-level "Cuentas" (those with IdCuentaPadre == null)
        var rootCuentas = cuentas.Where(c => c.IdCuentaPadre == null).ToList();

        // Convert root-level "Cuentas" to TreeItemData objects and build the tree structure
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
        // Find child "Cuentas" for the given parent "Cuenta"
        var childCuentas = cuentas.Where(c => c.IdCuentaPadre == parentCuenta.IdCuenta)
                                  .ToList();

        // Convert child "Cuentas" to TreeItemData objects and build the tree structure
        var treeItemChildren = new HashSet<TreeItemData>(childCuentas.Select(c =>
            new TreeItemData(c) { CuentasHijas = BuildTreeItemChildren(c, cuentas) }));

        return treeItemChildren;
    }

    private async Task LoadCuentas()
    {
        var cuentas    = await CuentaService.GetCuentasAsync(IdEmpresa);
        var rootCuenta = cuentas.FirstOrDefault(c => c.IdCuentaPadre == null);
        if (rootCuenta != null)
        {
            var treeRoot = CreateTree(rootCuenta, cuentas);
            TreeItems = new HashSet<TreeItemData> { treeRoot };
        }
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

    private void ToggleDrawer() => _open = !_open;

    private void CambiarEmpresa() => NavigationManager.NavigateTo("/inicio");

    private void NavigateToGestiones()
    {
        if (IdEmpresa is 0) return;
        var uri = $"/gestion/overview/{IdEmpresa}";
        NavigationManager.NavigateTo(uri);
    }
}