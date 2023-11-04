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

public class ActivityEndpoints : EndpointGroup
{
    [GopherEndpoint("/activity/{page}")]
    [GeminiEndpoint("/activity/{page}")]
    public List<GophermapItem> GetRecentActivity(RequestContext context, RefreshApiService apiService, BunkumConfig config, int page)
    {
        const int pageSize = 20;
        
        List<GophermapItem> map = new();
        map.AddHeading(context, "Recent Activity", 1);
        map.Add(new GophermapMessage(""));

        RefreshActivityPage activityPage = apiService.GetActivityPage((page - 1) * pageSize, pageSize);
        
        // TODO: STUPID STUPID STUPID STUPID STUPID STUPID STUPID
        int totalEvents = apiService.GetStatistics().statistics.TotalEvents;
        
        int maxPage = totalEvents / pageSize + 1;
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
                    map.AddHeading(context, $"{user.Username} {verb} {referencedUser.Username}", 2);
                    map.Add(new GophermapLink($"View {referencedUser.Username}'s profile", config, $"/user/{referencedUser.Username}"));
                    break;
                case RefreshEventDataType.Level:
                    RefreshLevel level = activityPage.Levels.First(l => l.LevelId == activityEvent.StoredSequentialId);
                    map.AddHeading(context, $"{user.Username} {verb} {level.Title}", 2);
                    map.Add(new GophermapLink($"View {level.Title}", config, $"/level/{level.LevelId}"));
                    break;
                case RefreshEventDataType.Score:
                    RefreshScore referencedScore = activityPage.Scores.First(u => u.ScoreId == activityEvent.StoredObjectId);
                    map.AddHeading(context, $"{user.Username} got a score of {referencedScore.Score} on {referencedScore.Level.Title}", 2);
                    map.Add(new GophermapLink($"View {referencedScore.Level.Title}", config, $"/level/{referencedScore.Level.LevelId}"));
                    break;
                case RefreshEventDataType.RateLevelRelation:
                    break;
                default:
                    throw new ArgumentOutOfRangeException();
            }
            
            map.Add(new GophermapLink($"View {user.Username}'s profile", config, $"/user/{user.Username}"));
            map.Add(new GophermapMessage(""));
        }
        
        map.Add(new GophermapMessage(""));
        map.AddHeading(context, $"You are on page {page}/{maxPage}", 3);
            
        if (page > 1)
            map.Add(new GophermapLink("First Page", config, $"/activity/1"));

        if(page != maxPage)
            map.Add(new GophermapLink("Next Page", config, $"/activity/{page + 1}"));
        
        if (page > 1)
            map.Add(new GophermapLink("Previous Page", config, $"/activity/{page - 1}"));
        
        if (page < maxPage)
            map.Add(new GophermapLink("Last Page", config, $"/activity/{maxPage}"));

        return map;
    }
}