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

public class PhotoEndpoints : EndpointGroup
{
    [GopherEndpoint("/photos/{page}")]
    [GeminiEndpoint("/photos/{page}")]
    public List<GophermapItem> Photos(RequestContext context, RefreshApiService apiService, BunkumConfig config, int page)
    {
        const int pageSize = 10;
        
        var photos = apiService.GetPhotos((page - 1) * pageSize, pageSize);

        List<GophermapItem> map = new();
        
        map.AddHeading(context, "Photos", 1);
        
        foreach (RefreshPhoto photo in photos.Items)
        {
            string displayText = $"Photo taken by {photo.Publisher.Username}";
            if (photo.Level != null)
            {
                displayText += $" on {photo.Level.Title}";
            }

            map.AddHeading(context, displayText, 3);
            map.Add(new GophermapMessage($"Taken at {photo.PublishedAt}"));
            map.Add(new GophermapLink(GophermapItemType.Image, "View Image", config, $"/photo/{photo.LargeHash}/photo.png"));
            map.Add(new GophermapLink("View User", config, $"/user/{photo.Publisher.Username}"));
            if (photo.Level != null)
            {
                map.Add(new GophermapLink("View Level", config, $"/level/{photo.Level.LevelId}"));
            }
            map.Add(new GophermapMessage(""));
        }
        
        int maxPage = photos.ListInfo.TotalItems / pageSize + 1;

        map.AddHeading(context, $"You are on page {page}/{maxPage}", 3);

        if (page > 1)
            map.Add(new GophermapLink("First Page", config, $"/photos/1"));

        if(page != maxPage)
            map.Add(new GophermapLink("Next Page", config, $"/photos/{page + 1}"));
        
        if (page > 1)
            map.Add(new GophermapLink("Previous Page", config, $"/photos/{page - 1}"));
        
        if (page < maxPage)
            map.Add(new GophermapLink("Last Page", config, $"/photos/{maxPage}"));
        
        return map;
    }

    [GopherEndpoint("/photo/{hash}/photo.png")]
    [GeminiEndpoint("/photo/{hash}/photo.png", ContentType.Png)]
    public byte[] GetPhoto(RequestContext context, RefreshApiService apiService, BunkumConfig config, string hash)
    {
        return apiService.GetImageData(hash);
    }
    
    [GopherEndpoint("/photo/psp/{hash}/photo.png")]
    [GeminiEndpoint("/photo/psp/{hash}/photo.png", ContentType.Png)]
    public byte[] GetPhotoPsp(RequestContext context, RefreshApiService apiService, BunkumConfig config, string hash)
    {
        return apiService.GetImageData("psp/" + hash);
    }
}