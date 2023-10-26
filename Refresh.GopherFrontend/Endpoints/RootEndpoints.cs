using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gemini;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;
using Refresh.GopherFrontend.Extensions;

namespace Refresh.GopherFrontend.Endpoints;

public class RootEndpoints : EndpointGroup
{
    [GopherEndpoint("/")]
    [GeminiEndpoint("/")]
    public List<GophermapItem> GetRoot(RequestContext context, RefreshApiService apiService, BunkumConfig config)
    {
        List<GophermapItem> map = new();

        if (context.IsGemini())
        {
            map.Add(new GophermapMessage($"# Welcome to {apiService.Instance.InstanceName}!"));
            map.Add(new GophermapMessage($"### {apiService.Instance.InstanceDescription}"));
        }
        else
        {
            map.Add(new GophermapMessage($"Welcome to {apiService.Instance.InstanceName}!"));
            map.Add(new GophermapMessage("    " + apiService.Instance.InstanceDescription));
        }

        map.AddRange(new GophermapItem[]
        {
            new GophermapMessage(""),
            new GophermapLink("About Server", config, "/statistics"),
            new GophermapLink("Levels", config, "/levels"),
            new GophermapLink("Recent Activity", config, "/activity/1"),
            new GophermapMessage(""),
        });

        if (apiService.Instance.Announcements.Any())
        {
            if (context.IsGemini())
            {
                map.Add(new GophermapMessage("## Announcements"));
                foreach (RefreshAnnouncement announcement in apiService.Instance.Announcements)
                {
                    map.Add(new GophermapMessage($"### {announcement.Title}"));
                    map.Add(new GophermapMessage(announcement.Text));
                }
            }
            else
            {
                map.Add(new GophermapMessage("=== ANNOUNCEMENTS ==="));
                foreach (RefreshAnnouncement announcement in apiService.Instance.Announcements)
                {
                    map.Add(new GophermapMessage($"*** {announcement.Title} ***"));
                    map.Add(new GophermapMessage("    " + announcement.Text));
                }
            }
        }
        return map;
    }
}