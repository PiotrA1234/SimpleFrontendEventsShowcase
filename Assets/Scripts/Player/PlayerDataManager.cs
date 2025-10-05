using Connection;
using Cysharp.Threading.Tasks;
using Shared;
using Shared.Requests;
using Zenject;

namespace Player
{
    public class PlayerDataManager : IPlayerDataManager
    {
        [Inject] 
        private IConnectionManager _connectionManager;
        public PlayerData Data { get; } = new();
        
        public PlayerDataManager()
        {
            Data = new();
        }

        public void Initialize()
        {
            _connectionManager.Subscribe<GetPlayerDataResponse>(OnResponse);
            _connectionManager.Subscribe<IncrementPlayerLevelResponse>(OnResponse);
            _connectionManager.SendRequestAsync(new GetPlayerDataRequest()).Forget();
        }
        
        public void IncrementLevel()
        {
            Data.Level.SetValue(Data.Level.Value + 1);
            _connectionManager.SendRequestAsync(new IncrementPlayerLevelRequest()).Forget();
        }

        private void OnResponse(GetPlayerDataResponse serverData)
        {
            SetLevel(serverData.level);
        }

        private void OnResponse(IncrementPlayerLevelResponse serverData)
        {
            SetLevel(serverData.level);
        }
        
        private void SetLevel(int level)
        {
            //if condition for the case of network lag, because messages can arrive out of order
            if (level > Data.Level.Value)
            {
                Data.Level.SetValue(level);
            }
        }
    }
}