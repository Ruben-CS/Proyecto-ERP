using BlazorFrontend.Pages.Comprobante.Editar;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Http.HttpResults;
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
        set => Articulo.PrecioVenta =
            decimal.TryParse(value, out var result) ? result : decimal.Zero;
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

    private void PopulateSelectedDetalels() => NombreCategorias =
        Detalles.Select(d => d.NombreCategoria).ToHashSet();

    private async Task Editar()
    {
        var updatedArticulo = new ArticuloDto
        {
            Nombre      = Articulo.Nombre,
            PrecioVenta = Articulo.PrecioVenta,
            Descripcion = Articulo.Descripcion,
            IdEmpresa   = IdEmpresa,
            IdArticulo  = IdArticulo,
            IdUsuario   = 1
        };

        var uri =
            $"https://localhost:44321/articulos/editarArticulo/{updatedArticulo.IdArticulo}";
        var response = await HttpClient.PutAsJsonAsync(uri, updatedArticulo);

        if (response.IsSuccessStatusCode)
        {
            await EditDetalles();
            Snackbar.Add("Articulo editado correctamente", Severity.Success);
            await OnArticuloAdded.InvokeAsync(updatedArticulo);
            MudDialog!.Close(DialogResult.Ok(response));
        }
    }

    private async Task EditDetalles()
    {
        //TODO make condition to check if the hashset has not changed to reduce workload on the api
        foreach (var nombre in NombreCategorias)
        {
            var idCategoria = Categorias.FirstOrDefault(c => c.Nombre == nombre)!
                                        .IdCategoria;
            var articuloCategoria = new ArticuloCategoriaDto()
            {
                IdArticulo      = IdArticulo,
                IdCategoria     = idCategoria,
                NombreCategoria = nombre
            };
            var uri =
                $"https://localhost:44321/articuloCategoria/editarArticuloDetalles/{IdArticulo}/{idCategoria}";
            var response = await HttpClient.PutAsJsonAsync(uri, articuloCategoria);
            response.EnsureSuccessStatusCode();
        }
    }

    private void Cancel() => MudDialog!.Cancel();
}