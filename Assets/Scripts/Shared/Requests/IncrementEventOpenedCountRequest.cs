using Connection;

namespace Shared.Requests
{
    public class IncrementEventOpenedCountRequest : IConnectionRequest
    {
        public readonly int uniqueEventId;

        public IncrementEventOpenedCountRequest(int uniqueEventId)
        {
            this.uniqueEventId = uniqueEventId;
        }
    }
}