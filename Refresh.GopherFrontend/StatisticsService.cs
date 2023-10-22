using System.Diagnostics;
using System.Reflection;
using Bunkum.Core.Database;
using Bunkum.Core.Responses;
using Bunkum.Core.Services;
using Bunkum.Listener.Request;
using NotEnoughLogs;

namespace Refresh.GopherFrontend;

public class StatisticsService : EndpointService
{
    public StatisticsService(Logger logger) : base(logger)
    {}

    public override void AfterRequestHandled(ListenerContext context, Response response, MethodInfo method, Lazy<IDatabaseContext> database)
    {
        switch (context.Protocol.Name)
        {
            case "Gopher":
                this.GopherRequestsServed++;
                break;
            case "Gemini":
                this.GeminiRequestsServed++;
                break;
            default:
                throw new Exception($"Unhandled protocol: {context.Protocol.Name} {context.Protocol.Version}");
        }
    }

    public DateTimeOffset StartTime { get; } = new();

    public int GopherRequestsServed { get; private set; }
    public int GeminiRequestsServed { get; private set; }
}