using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Articulo.Crear;

public partial class CrearArticulo
{
    private MudForm? _form;

    private bool _success;


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


    #endregion

    private async Task Crear()
    {

    }

    private void Cancel() => MudDialog!.Cancel();

}