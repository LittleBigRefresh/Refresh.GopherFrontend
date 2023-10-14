using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;

namespace Refresh.GopherFrontend.Endpoints;

public class LevelEndpoints : EndpointGroup
{
    [GopherEndpoint("/levels")]
    public List<GophermapItem> GetLevelCategories(RequestContext context, RefreshApiService apiService, BunkumConfig config)
    {
        ApiList<RefreshCategory> categories = apiService.GetLevelCategories();
        List<GophermapItem> map = new()
        {
            new GophermapMessage("Level Categories"),
            new GophermapMessage("")
        };
        
        foreach (RefreshCategory category in categories.Items)
        {
            if(category.RequiresUser) continue;
            if(category.ApiRoute == "search") continue;
            
            map.Add(new GophermapLink(category.Name, config, "/levels/" + category.ApiRoute));
            map.Add(new GophermapMessage(category.Description));
            map.Add(new GophermapMessage(""));
        }

        return map;
    }
}