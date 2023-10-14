using System.Net.Http.Json;
using Bunkum.Core.Services;
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
            BaseAddress = new Uri(config.RefreshApiUrl)
        };

        this.Instance = GetData<RefreshInstance>("instance");
    }

    private TResult GetData<TResult>(string endpoint)
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

        return result.Data;
    }
}