using System.Collections.ObjectModel;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Modelos.Models.Dtos;
using Modelos.Models.Enums;
using MudBlazor;

namespace BlazorFrontend.Pages.Nota.AgregarNota;

public partial class AgregarNotaCompra
{
    private bool     _success;
    private MudForm? _form;

    #region Parameters

    [Parameter]
    public int IdEmpresa { get; set; }

    #endregion

    #region Nota Details

    private string? NroNota { get; set; }

    private string? Descripcion { get; set; }

    private DateTime? Fecha { get; set; } = DateTime.Today;

    [CascadingParameter]
    private MudDialogInstance? MudDialog { get; set; } = new();

    #endregion

    private List<NotaDto> Notas { get; set; } = new();

    private List<ArticuloDto> Articulos { get; set; } = new();

    private ObservableCollection<LoteDto> DetalleParaLote = new();

    private async Task GoBack() =>
        await Task.FromResult(JsRuntime.InvokeVoidAsync("blazorBrowserHistory.goBack"));

    private void AddNewDetalleLote(LoteDto lote) => DetalleParaLote.Add(lote);


    private readonly DialogOptions _options = new()
    {
        MaxWidth             = MaxWidth.Large,
        DisableBackdropClick = true,
        Position             = DialogPosition.TopCenter
    };

    protected override async Task OnInitializedAsync()
    {
        Articulos = await ArticuloService.GetArticulosAsync(IdEmpresa);
        await GetNotaCompras();
        var nextNro = GetNextNumeroNota();
        NroNota = nextNro.ToString();
        await InvokeAsync(StateHasChanged);
    }

    private int GetNextNumeroNota()
    {
        var maxNroNota = Notas.Max(n => n.NroNota);
        return (maxNroNota ?? 0) + 1;
    }

    private async Task OpenAgregarDetalle()
    {
        var parameters = new DialogParameters
        {
            { "FechaIngreso", Fecha },
            { "Articulos", Articulos },
            { "IdEmpresa", IdEmpresa },
            {
                "AddNewDetalleLote",
                EventCallback.Factory.Create<LoteDto>(this, AddNewDetalleLote)
            },
            { "NroLote", int.Parse(NroNota) }
        };

        await DialogService.ShowAsync<AgregarDetalleModal>("Ingrese los detalles",
            parameters, _options);
    }


    private async Task AgregarCompra()
    {
        if (DetalleParaLote.Count == 0)
        {
            Snackbar.Add("Debe agregar al menos un articulo", Severity.Info);
            return;
        }

        var          total   = DetalleParaLote.Sum(d => d.PrecioCompra * d.Cantidad);
        const string url     = "https://localhost:44321/notas/agregarNota";
        var          nroNota = GetNextNumeroNota();
        var nota = new NotaDto()
        {
            NroNota       = nroNota,
            Fecha         = Fecha!.Value,
            Descripcion   = Descripcion!,
            Total         = total,
            IdEmpresa     = IdEmpresa,
            IdUsuario     = 1,
            IdComprobante = null,
            TipoNota      = TipoNota.Compra
        };
        var response = await HttpClient.PostAsJsonAsync(url, nota);
        if (response.IsSuccessStatusCode)
        {
            Snackbar.Add("Nota agregada exitosamente", Severity.Success);
            await AgregarLote();
        }
    }

    private async Task GetNotaCompras() =>
        Notas = (await NotaService.GetNotaComprasAsync(IdEmpresa))!;

    private async Task AgregarLote()
    {
        await GetNotaCompras();
        var ultimoIdNota = Notas.Last().IdNota;
        var url          = $"https://localhost:44321/lotes/agregarLote/{ultimoIdNota}";
        try
        {
            foreach (var lote in DetalleParaLote)

            {
                lote.IdNota = ultimoIdNota;
                var response = await HttpClient.PostAsJsonAsync(url, lote);
                response.EnsureSuccessStatusCode();
            }
            Snackbar.Add("Detalles guardados exitosamente", Severity.Success);
        }
        catch (Exception e)
        {
            Console.WriteLine($"Error occurred while posting details: {e.Message}");
            Snackbar.Add("Error al guardar los detalles", Severity.Error);
        }
    }
}