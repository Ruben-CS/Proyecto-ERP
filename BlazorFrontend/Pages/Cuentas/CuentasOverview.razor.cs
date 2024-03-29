using BlazorFrontend.Pages.Cuentas.Crear;
using BlazorFrontend.Pages.Cuentas.Editar;
using BlazorFrontend.Pages.Cuentas.Eliminar;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.Cuentas;

public partial class CuentasOverview
{
    private Dictionary<TreeItemData, HashSet<TreeItemData>>? RootItems { get; set; }

    [Inject]
    private IJSRuntime JSRuntime { get; set; }

    private TreeItemData? SelectedValue { get; set; }

    [Parameter]
    public EventCallback<TreeItemData> OnItemClicked { get; set; }

    private bool _folderOneExpanded = true;

    private List<CuentaDto> _cuentas = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    private HashSet<TreeItemData> TreeItems { get; set; } = new();

    public class TreeItemData
    {
        public int     IdCuenta { get; }
        public string? Codigo   { get; }
        public string  Nombre   { get; set; }

        public TipoCuenta TipoCuenta { get; }

        public int?                  IdCuentaPadre { get; set; }
        public HashSet<TreeItemData> CuentasHijas  { get; init; }

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

    private static TreeItemData CreateTree(TreeItemData treeItemData)
    {
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
            var children = CreateTree(rootItem);
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
        var cuentaDtos = cuentas.ToList();

        var childCuentas = cuentaDtos.Where(c => c.IdCuentaPadre == parentCuenta.IdCuenta)
                                     .ToList();
        var treeItemChildren = new HashSet<TreeItemData>(childCuentas
            .Select(c =>
                new TreeItemData(c)
                {
                    CuentasHijas = BuildTreeItemChildren(c, cuentaDtos)
                }));
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
                Snackbar.Configuration.PositionClass =
                    Defaults.Classes.Position.BottomRight;
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
        if (CheckLastHijo())
        {
            Snackbar.Add("Ya no puede crear mas hijos", Severity.Info);
            return;
        }

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

    private bool CheckLastHijo()
    {
        if (SelectedValue is not null)
        {
            var codigoSplitted = SelectedValue.Codigo!.Split('.');

            var lastPartIndex = codigoSplitted.Length - 1;
            var lastDigit     = codigoSplitted[lastPartIndex];

            var isLastPartDigit = int.TryParse(lastDigit, out var lastPartNumber) &&
                                  lastPartNumber > 0;
            return isLastPartDigit;
        }

        return false;
    }

    private async Task ShowEditarCuenta()
    {
        if (SelectedValue is null)
        {
            Snackbar.Add("Seleccione una cuenta primero", Severity.Info);
            return;
        }


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
        if (SelectedValue is null)
        {
            Snackbar.Add("Seleccione una cuenta primero", Severity.Info);
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
                EventCallback.Factory.Create<CuentaDto>(this, OnTreeViewChange)
            }
        };
        if (await HasChildren(SelectedValue))
        {
            Snackbar.Add("No puede eliminar una cuenta con hijos", Severity.Error);
        }
        else
        {
            await DialogService.ShowAsync<EliminarCuenta>
                ("Esta seguro?", parameters, options);
        }
    }

    private async Task OnTreeViewChange(CuentaDto cuentaDto)
    {
        _cuentas  = await CuentaService.GetCuentasAsync(IdEmpresa);
        TreeItems = BuildTreeItems(_cuentas);
        await LoadCuentas();
        await Task.FromResult(InvokeAsync(StateHasChanged));
    }

    private void GenerateReport()
    {
        var url =
            $"http://localhost:80/Reports/report/Report%20Project1/PlanDeCuentas?IdComprobante={IdEmpresa}";
        OpenUrlInNewTab(url);
    }

    private void OpenUrlInNewTab(string url)
    {
        var js = $"window.open('{url}', '_blank');";
        JSRuntime.InvokeVoidAsync("eval", js);
    }
}