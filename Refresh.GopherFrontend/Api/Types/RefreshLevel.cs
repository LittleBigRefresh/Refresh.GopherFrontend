namespace Refresh.GopherFrontend.Api.Types;

public class RefreshLevel
{
    public uint LevelId { get; set; }
    public RefreshUser? Publisher { get; set; }
    
#nullable disable
    
    public string Title { get; set; }
    public string Description { get; set; }
    public string IconHash { get; set; }
    
    public byte GameVersion { get; set; }
    
    public DateTimeOffset PublishDate { get; set; }
    public DateTimeOffset UpdateDate { get; set; }
    
    public bool TeamPicked { get; set; }
    public uint YayRatings { get; set; }
    public uint BooRatings { get; set; }
    public uint Hearts { get; set; }
    public uint UniquePlays { get; set; }
}