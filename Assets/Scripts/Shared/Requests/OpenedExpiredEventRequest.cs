using Connection;

namespace Shared.Requests
{
    public class OpenedExpiredEventRequest : IConnectionRequest
    {
        public readonly int uniqueEventId;
        
        public OpenedExpiredEventRequest(int uniqueEventId)
        {
            this.uniqueEventId = uniqueEventId;
        }
    }
}