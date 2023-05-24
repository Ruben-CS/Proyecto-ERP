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

    #endregion

    #region Form Fields

    public string? Nombre { get; set; }

    public decimal? Precio { get; set; }

    public string? Descripcion { get; set; }

    private IEnumerable<string> NombreCategorias { get; set; } = new HashSet<string>();

    #endregion

    private async Task Crear()
    {
    }

    public void Closed(MudChip chip)
    {
        var chipText = chip.Text;
        NombreCategorias = NombreCategorias.Where(c => c != chipText);
    }

    private void Cancel() => MudDialog!.Cancel();
}