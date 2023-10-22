using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gemini;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;

namespace Refresh.GopherFrontend.Endpoints;

public class LevelEndpoints : EndpointGroup
{
    [GopherEndpoint("/levels")]
    [GeminiEndpoint("/levels")]
    public List<GophermapItem> GetLevelCategories(RequestContext context, RefreshApiService apiService, BunkumConfig config)
    {
        ApiList<RefreshCategory> categories = apiService.GetLevelCategories();
        List<GophermapItem> map = new()
        {
            new GophermapMessage("Level Categories"),
            new GophermapMessage(""),
        };
        
        foreach (RefreshCategory category in categories.Items)
        {
            if(category.RequiresUser) continue;
            if(category.ApiRoute == "search") continue;
            
            map.Add(new GophermapLink(category.Name, config, $"/levels/{category.ApiRoute}/1"));
            map.Add(new GophermapMessage(category.Description));
        }

        return map;
    }

    [GopherEndpoint("/levels/{route}/{page}")]
    [GeminiEndpoint("/levels/{route}/{page}")]
    public List<GophermapItem> GetLevelListing(RequestContext context, RefreshApiService apiService, BunkumConfig config, string route, int page)
    {
        const int pageSize = 10;
        
        ApiList<RefreshLevel> levels = apiService.GetLevelListing(route, (page - 1) * pageSize, pageSize);
        
        int maxPage = levels.ListInfo.TotalItems / pageSize + 1;
        
        List<GophermapItem> map = new()
        {
            new GophermapMessage($"{route} Levels"),
            new GophermapMessage(""),
        };

        foreach (RefreshLevel level in levels.Items)
        {
            map.Add(new GophermapLink(level.Title.Length > 0 ? level.Title : "Unnamed Level", config, "/level/" + level.LevelId));
            if(level.Description.Length > 0)
                map.Add(new GophermapMessage(level.Description));
        }

        map.Add(new GophermapMessage(""));
        map.Add(new GophermapMessage($"You are on page {page}/{maxPage}"));
        
        if(page != maxPage)
            map.Add(new GophermapLink("Next Page", config, $"/levels/{route}/{page + 1}"));
        
        if (page > 1)
            map.Add(new GophermapLink("Previous Page", config, $"/levels/{route}/{page - 1}"));

        return map;
    }

    [GopherEndpoint("/level/{id}")]
    [GeminiEndpoint("/level/{id}")]
    public List<GophermapItem> GetLevel(RequestContext context, RefreshApiService apiService, BunkumConfig config, int id)
    {
        RefreshLevel level = apiService.GetLevel(id);
        List<GophermapItem> map = new()
        {
            new GophermapMessage(level.Title.Length > 0 ? level.Title : "Unnamed Level"),
            new GophermapMessage(level.Description.Length > 0 ? level.Description : "No description was provided for this level."),
        };

        if (level.Publisher != null)
        {
            map.Add(new GophermapMessage($"Published at {level.PublishDate} by {level.Publisher.Username}"));
            map.Add(new GophermapLink("View Publisher's Profile", config, "/user/" + level.Publisher.Username));
        }
        else
        {
            map.Add(new GophermapMessage($"Published at {level.PublishDate}"));
        }

        return map;
    }
}