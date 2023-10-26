using System.Text;
using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Listener.Protocol;
using Bunkum.Protocols.Gemini;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;
using Refresh.GopherFrontend.Extensions;

namespace Refresh.GopherFrontend.Endpoints;

public class LevelEndpoints : EndpointGroup
{
    // Use a separate lock for the categories list, this is due to _Categories sometimes being null, so we need something else to lock instead
    private static object _CategoriesLock = new();
    private static ApiList<RefreshCategory>? _Categories;
    
    [GopherEndpoint("/levels")]
    [GeminiEndpoint("/levels")]
    public List<GophermapItem> GetLevelCategories(RequestContext context, RefreshApiService apiService, BunkumConfig config)
    {
        ApiList<RefreshCategory> categories = apiService.GetLevelCategories();
        List<GophermapItem> map = new();
        
        if(context.IsGemini()) 
            map.Add(new GophermapMessage("# Level Categories"));
        else
        {
            map.Add(new GophermapMessage("Level Categories"));
            map.Add(new GophermapMessage(""));
        }

        foreach (RefreshCategory category in categories.Items)
        {
            if(category.RequiresUser) continue;
            if(category.ApiRoute == "search") continue;
            
            map.Add(new GophermapLink(category.Name, config, $"/levels/{category.ApiRoute}/1"));
            map.Add(new GophermapMessage(category.Description));
        }
        
        //Lock the global categories cache
        lock (_CategoriesLock)
        {
            //Set it to the new value
            _Categories = categories;
        }

        return map;
    }

    [GopherEndpoint("/levels/{route}/{page}")]
    [GeminiEndpoint("/levels/{route}/{page}")]
    public List<GophermapItem> GetLevelListing(RequestContext context, RefreshApiService apiService, BunkumConfig config, string route, int page)
    {
        const int pageSize = 10;

        string? categoryName = null;
        //Lock the global categories cache
        lock (_CategoriesLock)
        {
            //Ensure the categories cache is filled
            _Categories ??= apiService.GetLevelCategories();
            
            //Try to get the category name from the cache
            RefreshCategory? category = _Categories.Items.FirstOrDefault(c => c.ApiRoute == route);
            //If found, use the category name
            if (category != null) categoryName = category.Name;
        }
        //If we could not find the proper category name, default to the route
        categoryName ??= $"{route} Levels";
        
        ApiList<RefreshLevel> levels = apiService.GetLevelListing(route, (page - 1) * pageSize, pageSize);
        
        int maxPage = levels.ListInfo.TotalItems / pageSize + 1;
        
        List<GophermapItem> map = new();
        if (context.IsGemini())
            map.Add(new GophermapMessage($"# {categoryName}"));
        else
        {
            map.Add(new GophermapMessage(categoryName));
            map.Add(new GophermapMessage(""));
        }

        foreach (RefreshLevel level in levels.Items)
        {
            map.Add(new GophermapLink(level.Title.Length > 0 ? level.Title : "Unnamed Level", config, "/level/" + level.LevelId));
            if(level.Description.Length > 0)
                map.Add(new GophermapMessage(level.Description));
        }

        if(context.IsGemini()) 
            map.Add(new GophermapMessage($"### You are on page {page}/{maxPage}"));    
        else {
            map.Add(new GophermapMessage(""));
            map.Add(new GophermapMessage($"You are on page {page}/{maxPage}"));
        }

        if (page > 1)
            map.Add(new GophermapLink("First Page", config, $"/levels/{route}/1"));

        if(page != maxPage)
            map.Add(new GophermapLink("Next Page", config, $"/levels/{route}/{page + 1}"));
        
        if (page > 1)
            map.Add(new GophermapLink("Previous Page", config, $"/levels/{route}/{page - 1}"));
        
        if (page < maxPage)
            map.Add(new GophermapLink("Last Page", config, $"/levels/{route}/{maxPage}"));

        return map;
    }

    [GopherEndpoint("/level/{id}")]
    public List<GophermapItem> GetLevelGopher(RequestContext context, RefreshApiService apiService, BunkumConfig config, int id)
    {
        RefreshLevel level = apiService.GetLevel(id);
        List<GophermapItem> map = new();
        map.Add(new GophermapMessage(level.Title.Length > 0 ? level.Title : "Unnamed Level"));
        map.Add(new GophermapMessage(level.Description.Length > 0 ? level.Description : "No description was provided for this level."));

        if (level.Publisher != null)
        {
            map.Add(new GophermapMessage($"Published at {level.PublishDate} by {level.Publisher.Username}"));
            map.Add(new GophermapLink("View Publisher's Profile", config, "/user/" + level.Publisher.Username));
        }
        else
        {
            map.Add(new GophermapMessage($"Published at {level.PublishDate}"));
        }
        
        if (level.IconHash != "0" && level.IconHash[0] != 'g')
        {
            map.Add(new GophermapLink(GophermapItemType.Image, "View Level Icon", config, $"/level/{id}/icon.png"));
        }

        return map;
    }

    [GeminiEndpoint("/level/{id}")]
    public string GetLevelGemini(RequestContext context, RefreshApiService apiService, BunkumConfig config, int id)
    {
        RefreshLevel level = apiService.GetLevel(id);
        StringBuilder builder = new(1024);
        builder.AppendLine($"# {(level.Title.Length > 0 ? level.Title : "Unnamed Level")}");
        builder.AppendLine(level.Description.Length > 0 ? level.Description : "No description was provided for this level.");

        if (level.Publisher != null)
        {
            builder.AppendLine($"### Published at {level.PublishDate} by {level.Publisher.Username}");
            builder.AppendLine($"=> /user/{level.Publisher.Username} View Publisher's Profile");
        }
        else
        {
            builder.AppendLine($"### Published at {level.PublishDate}");
        }

        if (level.IconHash != "0" && level.IconHash[0] != 'g')
        {
            builder.AppendLine($"=> /level/{id}/icon.png View Level Icon");
        }

        return builder.ToString();
    }

    [GopherEndpoint("/level/{id}/icon.png")]
    [GeminiEndpoint("/level/{id}/icon.png", ContentType.Png)]
    public byte[] GetLevelIcon(RequestContext context, RefreshApiService apiService, int id)
    {
        RefreshLevel level = apiService.GetLevel(id);
        return apiService.GetImageData(level.IconHash);
    }
}