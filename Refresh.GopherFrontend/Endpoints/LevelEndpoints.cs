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
            
            map.Add(new GophermapLink(category.Name, config, $"/levels/{category.ApiRoute}/1"));
            map.Add(new GophermapMessage(category.Description));
            map.Add(new GophermapMessage(""));
        }

        return map;
    }

    [GopherEndpoint("/levels/{route}/{page}")]
    public List<GophermapItem> GetLevelListing(RequestContext context, RefreshApiService apiService, BunkumConfig config, string route, int page)
    {
        const int pageSize = 20;
        
        ApiList<RefreshLevel> levels = apiService.GetLevelListing(route, (page - 1) * pageSize, pageSize);
        
        int maxPage = levels.ListInfo.TotalItems / pageSize + 1;
        
        List<GophermapItem> map = new()
        {
            new GophermapMessage($"{route} Levels"),
            new GophermapLink("Return to Categories", config, "/levels"),
            new GophermapMessage(""),
        };

        foreach (RefreshLevel level in levels.Items)
        {
            map.Add(new GophermapLink(level.Title, config, "/level/" + level.LevelId));
            map.Add(new GophermapMessage(level.Description.TrimEnd()));
        }

        map.Add(new GophermapMessage(""));
        map.Add(new GophermapMessage($"You are on page {page}/{maxPage}"));
        
        if (page > 1)
            map.Add(new GophermapLink("Previous Page", config, $"/levels/{route}/{page - 1}"));
        
        if(page != maxPage)
            map.Add(new GophermapLink("Next Page", config, $"/levels/{route}/{page + 1}"));

        return map;
    }
}