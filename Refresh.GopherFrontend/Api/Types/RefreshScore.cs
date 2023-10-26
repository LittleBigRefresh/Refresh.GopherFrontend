namespace Refresh.GopherFrontend.Api.Types;

public class RefreshScore
{
    public string ScoreId { get; set; }
    public int Score { get; set; }
    public IEnumerable<RefreshUser> Players { get; set; }
    public DateTimeOffset ScoreSubmitted { get; set; }
    public byte ScoreType { get; set; }
    public RefreshLevel Level { get; set; }
}