namespace Refresh.GopherFrontend.Api.Types;

#nullable disable

public class RefreshInstance
{
    public required string InstanceName { get; set; }
    public required string InstanceDescription { get; set; }
    public required string SoftwareName { get; set; }
    public required string SoftwareVersion { get; set; }
    public required string SoftwareType { get; set; }
    
    public required IEnumerable<RefreshAnnouncement> Announcements { get; set; }
    public required bool MaintenanceModeEnabled { get; set; }
}