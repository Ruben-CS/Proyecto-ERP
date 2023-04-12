using BlazorFrontend.Services;
using Microsoft.AspNetCore.Components;
using MudBlazor;
using Modelos.Models.Dtos;

namespace BlazorFrontend.Pages.Empresa.Crear;

public partial class CrearEmpresa
{
    [Inject]
    ISnackbar Snackbar { get; set; } = null!;

    [Parameter]
    public EventCallback<EmpresaDto> OnEmpresaAdded { get; set; }

    [CascadingParameter]
    MudDialogInstance? MudDialog { get; set; }

    private          EmpresaDto EmpresaDto { get; } = new();
    private readonly List<int> _listaNiveles = Enumerable.Range(3, 5).ToList();
    private          IEnumerable<EmpresaDto> _empresaDtos = new List<EmpresaDto>();

    protected override async Task OnInitializedAsync()
    {
        _empresaDtos = await EmpresaService.GetEmpresasAsync();
    }

    private void Cancel() => MudDialog!.Cancel();

    private async Task UpsertEmpresa()
    {
        const string url = "https://localhost:44378/empresas/agregarempresa";

        var empresaDto = new EmpresaDto
        {
            Nombre    = EmpresaDto.Nombre,
            Nit       = EmpresaDto.Nit,
            Sigla     = EmpresaDto.Sigla,
            Telefono  = EmpresaDto.Telefono,
            Correo    = EmpresaDto.Correo,
            Direccion = EmpresaDto.Direccion,
            Niveles   = EmpresaDto.Niveles,
            IdUsuario = 1
        };

        if (!await ValidateUniqueNombre())
        {
            Snackbar.Add("Existe una empresa activa con ese nombre", Severity.Error);
        }
        else if (!await ValidateUniqueNit())
        {
            Snackbar.Add("Existe una empresa activa con el mismo NIT", Severity.Error);
        }
        else if (!await ValidateUniqueSigla())
        {
            Snackbar.Add("Existe una empresa activa con esas siglas", Severity.Error);
        }
        else if (await ValidateEmptyEmpresa())
        {
            Snackbar.Add("Rellene los datos esenciales", Severity.Error);
        }
        else
        {
            var response = await HttpClient.PostAsJsonAsync(url, empresaDto);
            Snackbar.Add("Empresa editada exitosamente", Severity.Success);
            var addedEmpresa = await response.Content.ReadFromJsonAsync<EmpresaDto>();
            await OnEmpresaAdded.InvokeAsync(addedEmpresa);
            MudDialog!.Close(DialogResult.Ok(response));
        }
    }

    private async Task<bool> ValidateUniqueNombre() =>
        await Task.FromResult(!_empresaDtos.Any(empresa =>
            empresa.Nombre    == EmpresaDto.Nombre &&
            empresa.IsDeleted == false
        ));

    private async Task<bool> ValidateUniqueNit() =>
        await Task.FromResult(!_empresaDtos.Any(empresa =>
            empresa.Nit       == EmpresaDto.Nit &&
            empresa.IsDeleted == false
        ));

    private async Task<bool> ValidateUniqueSigla() =>
        await Task.FromResult(!_empresaDtos.Any(empresa =>
            empresa.Sigla     == EmpresaDto.Sigla &&
            empresa.IsDeleted == false
        ));

    private async Task<bool> ValidateEmptyEmpresa() =>
        await Task.FromResult(string.IsNullOrEmpty(EmpresaDto.Nombre) ||
                              string.IsNullOrEmpty(EmpresaDto.Nit)    ||
                              string.IsNullOrEmpty(EmpresaDto.Sigla));
}