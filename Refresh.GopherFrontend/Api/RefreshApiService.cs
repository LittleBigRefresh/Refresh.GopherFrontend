using System.Diagnostics;
using System.Net;
using System.Net.Http.Json;
using Bunkum.Core.Services;
using JetBrains.Annotations;
using NotEnoughLogs;
using Refresh.GopherFrontend.Api.Types;
using Refresh.GopherFrontend.Configuration;

namespace Refresh.GopherFrontend.Api;

public class RefreshApiService : EndpointService
{
    private readonly HttpClient _client;
    public readonly RefreshInstance Instance;

    public RefreshApiService(GopherFrontendConfig config, Logger logger) : base(logger)
    {
        this._client = new HttpClient
        {
            BaseAddress = new Uri(config.RefreshApiUrl),
        };

        this.Instance = GetData<RefreshInstance>("instance");
    }

    private TResult GetData<TResult>(string endpoint) => this.GetData<TResult>(endpoint, out _);

    private TResult GetData<TResult>(string endpoint, out ApiListInformation? listInfo)
    {
        HttpResponseMessage response = _client.GetAsync(endpoint).Result;
        ApiResponse<TResult>? result = response.Content.ReadFromJsonAsync<ApiResponse<TResult>>().Result;
        if (result == null) throw new ApiErrorException("Result was unable to be parsed");

        if (!result.Success)
        {
            if (result.Error == null)
                throw new ApiErrorException($"Server sent invalid response for failure. Status code was {response.StatusCode}");
            
            throw new ApiErrorException(result.Error);
        }

        if (result.Data == null)
            throw new ApiErrorException($"Server sent invalid response for success. Status code was {response.StatusCode}");

        listInfo = result.ListInfo;
        return result.Data;
    }

    private byte[] GetBinaryData(string endpoint)
    {
        HttpResponseMessage response = _client.GetAsync(endpoint).Result;
        if (response.StatusCode != HttpStatusCode.OK)
        {
            ApiError apiResponse = response.Content.ReadFromJsonAsync<ApiError>().Result!;
            throw new ApiErrorException(apiResponse);
        } 
        
        return response.Content.ReadAsByteArrayAsync().Result;
    }

    public byte[] GetImageData(string hash) => GetBinaryData($"assets/{hash}/image");

    private ApiList<TResult> GetList<TResult>(string endpoint, int skip = 0, int count = 20)
    {
        IEnumerable<TResult> items = GetData<IEnumerable<TResult>>($"{endpoint}?skip={skip}&count={count}", out ApiListInformation? listInfo);
        Debug.Assert(listInfo != null, $"List information was not present on endpoint '/{endpoint}'");

        return new ApiList<TResult>
        {
            Items = items,
            ListInfo = listInfo,
        };
    }

    public (RefreshStatistics statistics, long latencyMs) GetStatistics()
    {
        Stopwatch stopwatch = new();
        stopwatch.Start();
        RefreshStatistics statistics = GetData<RefreshStatistics>("statistics");
        stopwatch.Stop();

        return (statistics, stopwatch.ElapsedMilliseconds);
    }

    public ApiList<RefreshCategory> GetLevelCategories() => GetList<RefreshCategory>("levels");
    public ApiList<RefreshLevel> GetLevelListing(string category, int skip = 0, int count = 20) => GetList<RefreshLevel>($"levels/{category}", skip, count);
    public RefreshLevel GetLevel(int id) => GetData<RefreshLevel>($"levels/id/{id}");
    public RefreshUser GetUser(string username) => GetData<RefreshUser>($"users/name/{username}");

    [Pure]
    public RefreshActivityPage GetActivityPage(int skip = 0, int count = 20)
    {
        return GetData<RefreshActivityPage>($"activity?count={count}&skip={skip}");
    }
}