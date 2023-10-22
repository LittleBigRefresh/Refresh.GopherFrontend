using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gemini;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;

namespace Refresh.GopherFrontend.Endpoints;

public class ActivityEndpoints : EndpointGroup
{
    [GopherEndpoint("/activity/{page}")]
    [GeminiEndpoint("/activity/{page}")]
    public List<GophermapItem> GetRecentActivity(RequestContext context, RefreshApiService apiService, int page)
    {
        const int pageSize = 20;
        
        List<GophermapItem> map = new()
        {
            new GophermapMessage("Recent Activity"),
            new GophermapMessage(""),
        };

        RefreshActivityPage activityPage = apiService.GetActivityPage(page * pageSize);
        foreach (RefreshActivityEvent activityEvent in activityPage.Events)
        {
            RefreshUser user = activityPage.Users.First(u => u.UserId == activityEvent.UserId);

            string verb = activityEvent.EventType switch
            {
                RefreshEventType.UploadLevel => "uploaded",
                RefreshEventType.FavouriteLevel => "favourited",
                RefreshEventType.UnfavouriteLevel => "unfavourited",
                RefreshEventType.FavouriteUser => "favourited",
                RefreshEventType.UnfavouriteUser => "unfavourited",
                RefreshEventType.PlayLevel => "played",
                RefreshEventType.TagLevel => "tagged",
                RefreshEventType.TeamPickLevel => "team picked",
                RefreshEventType.DpadRateLevel => "rated",
                RefreshEventType.ReviewLevel => "reviewed",
                RefreshEventType.SubmittedScore => "scored",
                RefreshEventType.FirstLogin => "logged in for the first time",
                _ => throw new ArgumentOutOfRangeException(),
            };
            
            switch (activityEvent.StoredDataType)
            {
                case RefreshEventDataType.User:
                    RefreshUser referencedUser = activityPage.Users.First(u => u.UserId == activityEvent.StoredObjectId);
                    map.Add(new GophermapMessage($"{user.Username} {verb} {referencedUser.Username}"));
                    break;
                case RefreshEventDataType.Level:
                    RefreshLevel level = activityPage.Levels.First(l => l.LevelId == activityEvent.StoredSequentialId);
                    map.Add(new GophermapMessage($"{user.Username} {verb} {level.Title}"));
                    break;
                case RefreshEventDataType.Score:
                    break;
                case RefreshEventDataType.RateLevelRelation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
        }

        return map;
    }
}