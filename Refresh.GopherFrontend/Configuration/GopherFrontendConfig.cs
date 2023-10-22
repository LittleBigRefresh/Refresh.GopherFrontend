using Bunkum.Core.Configuration;

namespace Refresh.GopherFrontend.Configuration;

public class GopherFrontendConfig : Config
{
    public override int Version { get; set; }
    public override int CurrentConfigVersion => 1;
    
    protected override void Migrate(int oldVer, dynamic oldConfig)
    {
        
    }

    public string RefreshApiUrl { get; set; } = "https://littlebigrefresh.com/api/v3/";
}