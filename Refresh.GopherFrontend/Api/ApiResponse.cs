namespace Refresh.GopherFrontend.Api;

public class ApiResponse<TData>
{
    public bool Success { get; set; }
    public TData? Data { get; set; }
    public ApiError? Error { get; set; }
}