using Connection;
using LOEvents;

namespace Shared.Requests
{
    public class GetActiveLOEventsRequest : IConnectionRequest
    {
        public readonly LOEventsMockData data;
        
        public GetActiveLOEventsRequest(LOEventsMockData data)
        {
            this.data = data;
        }
    }
}