using Bunkum.Core;
using Bunkum.Core.Endpoints;
using Bunkum.Protocols.Gopher;
using Bunkum.Protocols.Gopher.Responses;

namespace Refresh.GopherFrontend.Endpoints;

public class RootEndpoints : EndpointGroup
{
    [GopherEndpoint("/")]
    public List<GophermapItem> GetRoot(RequestContext context)
    {
        return new List<GophermapItem>();
    }
}