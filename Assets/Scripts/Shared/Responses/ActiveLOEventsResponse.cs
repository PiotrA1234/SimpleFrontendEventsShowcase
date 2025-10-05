using Connection;
using Shared.Types;

namespace Shared
{
    public class ActiveLOEventsResponse : IConnectionResponse
    {
        public LOEventBaseData[] Events;
    }
}