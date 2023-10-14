namespace Refresh.GopherFrontend.Api;

public class ApiList<TData>
{
    public ApiListInformation ListInfo { get; set; }
    public IEnumerable<TData> Items { get; set; }
}