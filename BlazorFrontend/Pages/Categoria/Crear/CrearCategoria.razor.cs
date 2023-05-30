using Microsoft.AspNetCore.Components;
using Modelos.Models.Dtos;
using MudBlazor;

namespace BlazorFrontend.Pages.Categoria.Crear;

public partial class CrearCategoria
{
    private CategoriaDto CategoriaDto { get; } = new();

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; }

    [Parameter]
    public int IdEmpresa { get; set; }

    [Parameter]
    public EventCallback<CategoriaDto> OnTreeViewChange { get; set; }

    [Parameter]
    public TreeItemDataCategoria SelectedValue { get; set; }

    private List<CategoriaDto>? _categorias = new();

    private async Task CreateCuenta()
    {
        const string url = "https://localhost:44321/categorias/agregarCategoria";

        var categoriaDto = new CategoriaDto
        {
            Nombre           = CategoriaDto.Nombre,
            IdCategoriaPadre = SelectedValue.IdCategoria,
            Descripcion      = CategoriaDto.Descripcion,
            IdEmpresa        = IdEmpresa,
            IdUsuario        = 1,
            Estado           = true
        };
        if (await ValidateName(categoriaDto))
        {
            Snackbar.Add("Este nombre ya existe", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, categoriaDto);
            Snackbar.Add("Cuenta creada exitosamente", Severity.Success);
            await OnTreeViewChange.InvokeAsync(categoriaDto);
            MudDialog!.Close(DialogResult.Ok(response));
        }
    }

    protected override async Task OnInitializedAsync()
    {
        _categorias = await CategoriaService.GetCategoriasService(IdEmpresa);
    }

    private async Task<bool> ValidateName(CategoriaDto cuentaDto)
    {
        return await Task.FromResult(_categorias.Any(c =>
            c.Nombre    == cuentaDto.Nombre &&
            c.IdEmpresa == cuentaDto.IdEmpresa));
    }


    private void Cancel() => MudDialog!.Cancel();
}