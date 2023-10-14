namespace Refresh.GopherFrontend.Api.Types;

#nullable disable

public class RefreshAnnouncement
{
    public string AnnouncementId { get; set; }
    public string Title { get; set; }
    public string Text { get; set; }
    public DateTimeOffset CreatedAt { get; set; }
}