using Bunkum.Core;
using Bunkum.Core.Configuration;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gemini;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;
using Bunkum.Protocols.Gopher.Responses.Items;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Api.Types;
using Refresh.GopherFrontend.Configuration;

namespace Refresh.GopherFrontend.Endpoints;

public class StatisticsEndpoints : EndpointGroup
{
    [GopherEndpoint("/statistics")]
    [GeminiEndpoint("/statistics")]
    public List<GophermapItem> GetStatistics(RequestContext context,
        RefreshApiService apiService,
        StatisticsService statisticsService,
        GopherFrontendConfig frontendConfig,
        BunkumConfig config)
    {
        RefreshStatistics statistics = apiService.GetStatistics();
        RefreshRequestStatistics requests = statistics.RequestStatistics;
        return new List<GophermapItem>
        {
            new GophermapMessage($"Statistics for {apiService.Instance.InstanceName}"),
            new GophermapMessage($"    Server software: {apiService.Instance.SoftwareName} ({apiService.Instance.SoftwareType})"),
            new GophermapMessage($"    Server version: v{apiService.Instance.SoftwareVersion}"),
            new GophermapLink($"    API base URL in use: {frontendConfig.RefreshApiUrl}", new Uri(frontendConfig.RefreshApiUrl)),
            new GophermapMessage(""),
            new GophermapMessage("Things!"),
            new GophermapMessage($"    Registered users: {statistics.TotalUsers:N0}"),
            new GophermapMessage($"    Submitted levels: {statistics.TotalLevels:N0}"),
            new GophermapMessage($"    Uploaded photos: {statistics.TotalPhotos:N0}"),
            new GophermapMessage($"    Events occurred: {statistics.TotalEvents:N0}"),
            new GophermapMessage(""),
            new GophermapMessage($"Served requests ({requests.TotalRequests:N0} in total)"),
            new GophermapMessage($"    API Requests: {requests.ApiRequests:N0}"),
            new GophermapMessage($"    Game API Requests: {requests.GameRequests:N0}"),
            new GophermapMessage($"    Legacy API Requests: {requests.LegacyApiRequests:N0}"),
            new GophermapMessage(""),
            new GophermapMessage("Activity"),
            new GophermapMessage($"    People online now: {statistics.CurrentIngamePlayersCount:N0}"),
            new GophermapMessage($"    Active rooms: {statistics.CurrentRoomCount:N0}"),
            new GophermapMessage(""),
            new GophermapMessage("Statistics for this proxy"),
            new GophermapLink("    Source code (GitHub)", new Uri("https://github.com/LittleBigRefresh/Refresh.GopherFrontend")),
            new GophermapMessage($"    Protocol in use: {context.Protocol.Name} {context.Protocol.Version}"),
            new GophermapMessage($"    Gopher requests served: {statisticsService.GopherRequestsServed:N0}"),
            new GophermapMessage($"    Gemini requests served: {statisticsService.GeminiRequestsServed:N0}"),
        };
    }
}