namespace ModuloContabilidadApi.Models.Dtos;

public class ResponseDto
{
    public bool         IsSucces       { get; set; }
    public object       Result         { get; set; }
    public string       DisplayMessage { get; set; }
    public List<string> ErrorMessages  { get; set; }
}