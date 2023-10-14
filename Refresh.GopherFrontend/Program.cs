using System.Reflection;
using Bunkum.Core.Responses;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses.Serialization;
using NotEnoughLogs;
using NotEnoughLogs.Behaviour;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Configuration;

Response.AddSerializer<BunkumGophermapSerializer>();

BunkumGopherServer server = new(new LoggerConfiguration
{
    Behaviour = new QueueLoggingBehaviour(),
    MaxLevel = LogLevel.Trace,
});

server.Initialize = s =>
{
    s.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    s.AddConfigFromJsonFile<GopherFrontendConfig>("gopherFrontend.json");
    s.AddService<RefreshApiService>();
};

server.Start();
await Task.Delay(-1);