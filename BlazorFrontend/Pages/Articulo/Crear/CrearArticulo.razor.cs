using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Articulo.Crear;

public partial class CrearArticulo
{
    private MudForm? _form;

    private bool _success;

    private string Value { get; set; } = string.Empty;


    #region Params

    [Parameter]
    public List<CategoriaDto> CategoriaDtos { get; set; } = new();

    [Parameter]
    public int IdEmpresa { get; set; }

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public EventCallback<ArticuloDto> OnArticuloAdded { get; set; }

    #endregion

    #region Form Fields

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    private string PrecioString
    {
        get => Precio?.ToString("F2") ?? string.Empty;
        set
        {
            if (decimal.TryParse(value, out var result))
            {
                Precio = result;
            }
            else
            {
                Precio = null;
            }
        }
    }

    public string? Descripcion { get; set; }

    private IEnumerable<string> NombreCategorias { get; set; } = new HashSet<string>();

    #endregion

    protected override async Task OnInitializedAsync()
    {
        Snackbar.Configuration.PositionClass = Defaults.Classes.Position.BottomLeft;
    }

    private async Task Crear()
    {
        const string url = "https://localhost:44321/articulos/agregarArticulo";
        var articulo = new ArticuloDto
        {
            Nombre      = Nombre,
            Descripcion = Descripcion,
            PrecioVenta = Precio!.Value,
            IdEmpresa   = IdEmpresa,
            IdUsuario   = 1
        };
        var response = await HttpClient.PostAsJsonAsync(url, articulo);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Articulo creado exitosamente", Severity.Success);
            await CrearDetalle();
            MudDialog!.Close(DialogResult.Ok(response));
            await OnArticuloAdded.InvokeAsync(articulo);
        }
    }

    private async Task CrearDetalle()
    {
        var articuloDtos =
            await ArticuloService.GetArticulosAsync(IdEmpresa);
        var lastArticleCreatedId = articuloDtos.Last().IdArticulo;
        foreach (var nombre in NombreCategorias)
        {
            var idCategoria = CategoriaDtos.FirstOrDefault(c => c.Nombre == nombre)!
                                           .IdCategoria;
            var articuloCategoria = new ArticuloCategoriaDto
            {
                IdArticulo      = lastArticleCreatedId,
                IdCategoria     = idCategoria,
                NombreCategoria = nombre
            };
            var url =
                $"https://localhost:44321/articuloCategoria/addArticuloCategoria/{lastArticleCreatedId}/{idCategoria}";
            var response = await HttpClient.PostAsJsonAsync(url, articuloCategoria);
            response.EnsureSuccessStatusCode();
        }
    }

    public void Closed(MudChip chip)
    {
        var chipText = chip.Text;
        NombreCategorias = NombreCategorias.Where(c => c != chipText);
    }


    private void Cancel() => MudDialog!.Cancel();
}