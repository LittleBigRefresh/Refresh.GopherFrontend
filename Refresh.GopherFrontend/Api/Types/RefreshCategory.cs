namespace Refresh.GopherFrontend.Api.Types;

public class RefreshCategory
{
    public string Name { get; set; }
    public string Description { get; set; }
    public string ApiRoute { get; set; }
    public bool RequiresUser { get; set; }
}