using System.Net;

namespace Refresh.GopherFrontend.Api;

public class ApiError
{
    public string Name { get; set; }
    public string Message { get; set; }
    public HttpStatusCode StatusCode { get; set; }
}