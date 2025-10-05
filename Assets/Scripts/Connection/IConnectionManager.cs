using Cysharp.Threading.Tasks;

namespace Connection
{
    public interface IConnectionManager
    {
        void Subscribe<T>(System.Action<T> callback) where T: IConnectionResponse;

        void Unsubscribe<T>(System.Action<T> callback) where T: IConnectionResponse;

        UniTask SendRequestAsync<T>(T fakeMockupRequest) where T: IConnectionRequest;
    }
}