namespace Refresh.GopherFrontend.Api;

public class ApiErrorException : Exception
{
    public ApiError? Error { get; }

    public ApiErrorException(ApiError error) : base($"{error.Name}: {error.Message} ({error.StatusCode})")
    {
        this.Error = error;
    }

    public ApiErrorException(string message) : base(message)
    {
        this.Error = null;
    }
}