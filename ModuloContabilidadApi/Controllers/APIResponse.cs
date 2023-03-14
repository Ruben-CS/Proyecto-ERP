using System.Net;
using Microsoft.AspNetCore.Authorization;

namespace ModuloContabilidadApi.Controllers;

public class APIResponse
{
    public APIResponse()
    {
        ErrorMessages = new List<string>();
    }

    public bool IsSucces { get; set; }

    public object Result { get; set; }

    public HttpStatusCode StatusCode { get; set; }

    public List<string> ErrorMessages { get; set; }
}