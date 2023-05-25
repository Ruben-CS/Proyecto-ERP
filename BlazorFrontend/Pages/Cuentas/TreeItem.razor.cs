using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
namespace BlazorFrontend.Pages.Cuentas;

public partial class TreeItem
{
    private bool _folderOneExpanded = true;
    private bool _folderTwoExpanded;

    private CuentasOverview.TreeItemData SelectedValue { get; set; }

    private void OnItemClick(CuentasOverview.TreeItemData value)
    {
        SelectedValue = value;
    }
}