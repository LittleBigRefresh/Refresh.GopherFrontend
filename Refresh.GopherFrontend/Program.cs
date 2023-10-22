using System.Reflection;
using Bunkum.Protocols.Gemini;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses.Serialization;
using Bunkum.Serialization.GopherToGemini;
using NotEnoughLogs;
using NotEnoughLogs.Behaviour;
using Refresh.GopherFrontend.Api;
using Refresh.GopherFrontend.Configuration;

LoggerConfiguration logConfig = new LoggerConfiguration
{
    Behaviour = new QueueLoggingBehaviour(),
    MaxLevel = LogLevel.Trace,
};

BunkumGopherServer gopherServer = new(logConfig);
BunkumGeminiServer geminiServer = new(null, logConfig);

gopherServer.Initialize = s =>
{
    s.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    s.AddConfigFromJsonFile<GopherFrontendConfig>("gopherFrontend.json");
    s.AddService<RefreshApiService>();
    
    s.AddSerializer<BunkumGophermapSerializer>();
};

geminiServer.Initialize = s =>
{
    s.DiscoverEndpointsFromAssembly(Assembly.GetExecutingAssembly());
    s.AddConfigFromJsonFile<GopherFrontendConfig>("gopherFrontend.json");
    s.AddService<RefreshApiService>();

    s.AddSerializer<BunkumGophermapGeminiSerializer>();
};

gopherServer.Start();
geminiServer.Start();

await Task.Delay(-1);