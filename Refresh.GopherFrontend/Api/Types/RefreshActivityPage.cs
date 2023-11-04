namespace Refresh.GopherFrontend.Api.Types;

public class RefreshActivityPage
{
    public IEnumerable<RefreshActivityEvent> Events { get; set; }
    public IEnumerable<RefreshUser> Users { get; set; }
    public IEnumerable<RefreshLevel> Levels { get; set; }
    public IEnumerable<RefreshScore> Scores { get; set; }
}