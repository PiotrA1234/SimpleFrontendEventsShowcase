using Connection;

namespace Shared
{
    public class OpenedExpiredEventResponse : IConnectionResponse
    {
        public readonly int uniqueId;
        
        public OpenedExpiredEventResponse(int uniqueId)
        {
            this.uniqueId = uniqueId;
        }
    }
}