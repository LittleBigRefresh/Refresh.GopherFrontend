namespace Refresh.GopherFrontend.Api.Types;

public class RefreshUser
{
    public string UserId { get; set; }
    public string Username { get; set; }
    public string IconHash { get; set; }
    public string Description { get; set; }
    public DateTimeOffset JoinDate { get; set; }
}