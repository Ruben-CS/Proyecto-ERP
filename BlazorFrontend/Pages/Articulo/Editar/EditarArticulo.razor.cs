using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Articulo.Editar;

public partial class EditarArticulo
{
    private MudForm? _form;

    private bool _success;


    #region Parameters

    [Parameter]
    public int IdArticulo { get; set; }

    [Parameter]
    public EventCallback<ArticuloDto> OnArticuloAdded { get; set; }


    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public ArticuloDto Articulo { get; set; } = new();

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public List<CategoriaDto> Categorias { get; set; } = new();

    #endregion

    private List<ArticuloCategoriaDto> Detalles { get; set; } = new();

    private string PrecioString
    {
        get => Articulo.PrecioVenta.ToString("F2");
        set => Articulo.PrecioVenta = decimal.TryParse(value, out var result) ? result : decimal.Zero;
    }


    private IEnumerable<string> NombreCategorias { get; set; } = new HashSet<string>();
    private string              Value            { get; set; } = string.Empty;

    protected override async Task OnInitializedAsync()
    {
        Detalles = await ArticuloCategoriaService.GetArticuloCategoriasAsync(IdArticulo);
        PopulateSelectedDetalels();
        await InvokeAsync(StateHasChanged);
    }

    private void Closed(MudChip chip)
    {
        var chipText = chip.Text;
        NombreCategorias = NombreCategorias.Where(c => c != chipText);
    }

    private void PopulateSelectedDetalels()
    {
        NombreCategorias =  Detalles.Select(d => d.NombreCategoria).ToHashSet();
    }

    private async Task Editar()
    {

    }

    private void Cancel() => MudDialog!.Cancel();

}