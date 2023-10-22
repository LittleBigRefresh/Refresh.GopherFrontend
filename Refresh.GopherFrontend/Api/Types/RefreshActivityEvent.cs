namespace Refresh.GopherFrontend.Api.Types;

public class RefreshActivityEvent
{
    public string EventId { get; set; }
    public RefreshEventType EventType { get; set; }
    
    public string UserId { get; set; }
    
    public DateTimeOffset OccuredAt { get; set; }
    
    public RefreshEventDataType StoredDataType { get; set; }
    public int? StoredSequentialId { get; set; }
    public string? StoredObjectId { get; set; }
}