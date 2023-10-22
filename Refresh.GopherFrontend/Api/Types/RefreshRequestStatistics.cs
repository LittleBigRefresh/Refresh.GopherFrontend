namespace Refresh.GopherFrontend.Api.Types;

public class RefreshRequestStatistics
{
    public long TotalRequests { get; set; }
    public long ApiRequests { get; set; }
    public long LegacyApiRequests { get; set; }
    public long GameRequests { get; set; }
}