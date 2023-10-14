using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;

namespace Refresh.GopherFrontend.Endpoints;

public class RootEndpoints : EndpointGroup
{
    [GopherEndpoint("/")]
    public List<GophermapItem> GetRoot(RequestContext context, RefreshApiService apiService, BunkumConfig config)
    {
        List<GophermapItem> map = new List<GophermapItem>
        {
            new GophermapMessage($"Welcome to the {apiService.Instance.InstanceName} Gopher Frontend!"),
            new GophermapMessage("    " + apiService.Instance.InstanceDescription),
            new GophermapMessage(""),
            new GophermapLink("Server Statistics", config, "/statistics"),
            new GophermapMessage(""),
        };

        map.Add(new GophermapMessage("=== ANNOUNCEMENTS ==="));
        foreach (RefreshAnnouncement announcement in apiService.Instance.Announcements)
        {
            map.Add(new GophermapMessage($"*** {announcement.Title} ***"));
            map.Add(new GophermapMessage("    " + announcement.Text));
        }

        return map;
    }
}