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

public class StatisticsEndpoints : EndpointGroup
{
    [GopherEndpoint("/statistics")]
    [GeminiEndpoint("/statistics")]
    public List<GophermapItem> GetStatistics(RequestContext context, RefreshApiService apiService, BunkumConfig config)
    {
        RefreshStatistics statistics = apiService.GetStatistics();
        return new List<GophermapItem>
        {
            new GophermapMessage($"Statistics for {apiService.Instance.InstanceName}"),
            new GophermapMessage($"    Server software: {apiService.Instance.SoftwareName} ({apiService.Instance.SoftwareType})"),
            new GophermapMessage($"    Server version: v{apiService.Instance.SoftwareVersion}"),
            new GophermapMessage(""),
            new GophermapMessage($"Registered users: {statistics.TotalUsers}"),
            new GophermapMessage($"Submitted levels: {statistics.TotalLevels}"),
            new GophermapMessage($"Uploaded photos: {statistics.TotalPhotos}"),
            new GophermapMessage($"Events occurred: {statistics.TotalEvents}"),
            new GophermapMessage(""),
            new GophermapMessage($"People online now: {statistics.CurrentIngamePlayersCount}"),
            new GophermapMessage($"Active rooms: {statistics.CurrentRoomCount}"),
        };
    }
}