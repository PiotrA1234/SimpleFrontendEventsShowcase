using System;
using System.Linq;
using System.Threading;
using Connection;
using Cysharp.Threading.Tasks;
using RuntimeBindings;
using Shared;
using Shared.Requests;
using Shared.Types;
using Unity.VisualScripting;
using UnityEngine.AddressableAssets;

namespace LOEvents
{
    public class LOEventsManager : ILOEventsManager
    {
        private IConnectionManager _connectionManager;
        private UpdatableField<LOEventBaseData[]> _data = new();
        private LOEventsMockData _eventsMockData;
        private CancellationTokenSource _cts = new();
        public IReadOnlyUpdatableField<LOEventBaseData[]> Data => _data;
        
        public LOEventsManager(IConnectionManager connectionManager)
        {
            _connectionManager = connectionManager;
        }
        
        public void OnEventOpened(LOEventBaseData eventData)
        {
            eventData.openedCount++;
            _connectionManager.SendRequestAsync(new IncrementEventOpenedCountRequest(eventData.uniqueId)).Forget();
        }

        public void OnOpenedExpiredEvent(LOEventBaseData eventData)
        {
            eventData.openedAfterExpired = true;
            _connectionManager.SendRequestAsync(new OpenedExpiredEventRequest(eventData.uniqueId));
        }

        public void Initialize()
        {
            _data.SetValue(Array.Empty<LOEventBaseData>());
            _connectionManager.Subscribe<ActiveLOEventsResponse>(OnResponse);
            LoadSettings().Forget();
        }

        private void OnResponse(ActiveLOEventsResponse response)
        {
            _data.SetValue(response.Events.DistinctBy(e => e.uniqueId).ToArray());
        }

        private async UniTask LoadSettings()
        {
            var handle = Addressables.LoadAssetAsync<LOEventsMockData>(LO_EVENTS_MOCK_DATA_KEY);
            await handle.ToUniTask(cancellationToken: _cts.Token);
            if (!_cts.IsCancellationRequested)
            {
                _eventsMockData = handle.Result;
                _connectionManager.SendRequestAsync(new GetActiveLOEventsRequest(_eventsMockData)).Forget();
            }
            
            handle.Release();
        }

        private const string LO_EVENTS_MOCK_DATA_KEY = "LOEventsMockData";
    }
}