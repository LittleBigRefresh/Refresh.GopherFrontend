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

public class UserEndpoints : EndpointGroup
{
    [GopherEndpoint("/user/{username}")]
    [GeminiEndpoint("/user/{username}")]
    public List<GophermapItem> GetUserProfile(RequestContext context,
        RefreshApiService apiService,
        BunkumConfig config,
        string username)
    {
        RefreshUser user = apiService.GetUser(username);
        
        List<GophermapItem> map = new();
        map.AddHeading(context, $"{user.Username}'s Profile", 1);
        map.Add(new GophermapMessage(""));
        map.AddHeading(context, user.Description, 2);
        map.AddHeading(context, $"Joined on {user.JoinDate}", 3);

        map.Add(new GophermapLink("View Uploaded Levels", config, $"/levels/byUser/{user.Username}/1"));
        
        if (user.IconHash != "0" && user.IconHash[0] != 'g')
        {
            map.Add(new GophermapLink(GophermapItemType.Image, "View User's Avatar", config, $"/user/{username}/avatar.png"));
        }

        return map;
    }
    
    [GopherEndpoint("/user/{username}/avatar.png")]
    [GeminiEndpoint("/user/{username}/avatar.png", ContentType.Png)]
    public byte[] GetLevelIcon(RequestContext context, RefreshApiService apiService, string username)
    {
        RefreshUser user = apiService.GetUser(username);
        return apiService.GetImageData(user.IconHash);
    }
}