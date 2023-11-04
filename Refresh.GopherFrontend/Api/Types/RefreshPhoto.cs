namespace Refresh.GopherFrontend.Api.Types;

public class RefreshPhoto
{
    public int PhotoId { get; set; }
    public DateTimeOffset TakenAt { get; set; }
    public DateTimeOffset PublishedAt { get; set; }
    
    public RefreshUser Publisher { get; set; }
    public RefreshLevel? Level { get; set; }
    
    public string LevelName { get; set; }
    public string LevelType { get; set; }
    public int LevelId { get; set; }
    
    public string SmallHash { get; set; }
    public string MediumHash { get; set; }
    public string LargeHash { get; set; }
    public string PlanHash { get; set; } 
}