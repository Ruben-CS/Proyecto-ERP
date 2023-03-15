namespace ModuloContabilidadApi.Models;

public class ResponseDto
{
    public bool         IsSucces       { get; set; } = true;
    public object       Result         { get; set; }
    public string       DisplayMessage { get; set; } = string.Empty;
    public List<string> ErrorMessages  { get; set; }
}