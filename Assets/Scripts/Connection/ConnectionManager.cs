using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Cysharp.Threading.Tasks;
using Player;
using Shared;
using Shared.Requests;
using UnityEngine;
using Random = UnityEngine.Random;

namespace Connection
{
    public class ConnectionManager : IConnectionManager
    {
        private readonly Dictionary<Type, Delegate> _subscribers = new();
        
        public void Subscribe<T>(Action<T> callback) where T : IConnectionResponse
        {
            if (callback == null) return;

            if (_subscribers.TryGetValue(typeof(T), out var existing))
            {
                _subscribers[typeof(T)] = Delegate.Combine(existing, callback);
            }
            else
            {
                _subscribers[typeof(T)] = callback;
            }
        }

        public void Unsubscribe<T>(Action<T> callback) where T : IConnectionResponse
        {
            if (callback == null) return;

            if (_subscribers.TryGetValue(typeof(T), out var existing))
            {
                var newDelegate = Delegate.Remove(existing, callback);
                if (newDelegate == null)
                    _subscribers.Remove(typeof(T));
                else
                    _subscribers[typeof(T)] = newDelegate;
            }
        }

        public async UniTask SendRequestAsync<T>(T fakeMockupRequest) where T: IConnectionRequest
        {
            // here we would normally send the request to the server and wait for response
            // but for sake of this task - PlayerPrefs serve as the servers data and we cast the fakeMockupRequest explicitly to fake the responses
            if (typeof(T) == typeof(IncrementPlayerLevelRequest))
            {
                var playerLevel = PlayerPrefs.GetInt(PLAYER_LEVEL_KEY, 1);
                playerLevel++;
                PlayerPrefs.SetInt(PLAYER_LEVEL_KEY, playerLevel);
                
                var fakeResponse = new IncrementPlayerLevelResponse();
                fakeResponse.level = playerLevel;
                ReceiveMessage(fakeResponse);
            }
            else if (typeof(T) == typeof(OpenedExpiredEventRequest))
            {
                var uniqueEventId = (fakeMockupRequest as OpenedExpiredEventRequest).uniqueEventId;
                PlayerPrefs.SetInt(GetEventOpenedAfterExpiredPrefsKey(uniqueEventId), 1);

                var fakeResponse = new OpenedExpiredEventResponse(uniqueEventId);
                ReceiveMessage(fakeResponse);
            }
            else if (typeof(T) == typeof(IncrementEventOpenedCountRequest))
            {
                var uniqueEventId = (fakeMockupRequest as IncrementEventOpenedCountRequest).uniqueEventId;
                var eventOpenedCount = GetEventOpenedCount(uniqueEventId);
                eventOpenedCount++;
                SaveEventOpenedCount(uniqueEventId, eventOpenedCount);
                
                var fakeResponse = new IncrementEventOpenedCountResponse();
                fakeResponse.openedCount = eventOpenedCount;
                ReceiveMessage(fakeResponse);
            }
            else if(typeof(T) == typeof(GetPlayerDataRequest))
            {
                var playerLevel = PlayerPrefs.GetInt(PLAYER_LEVEL_KEY, 1);
                
                var fakeResponse = new IncrementPlayerLevelResponse();
                fakeResponse.level = playerLevel;
                ReceiveMessage(fakeResponse);
            }
            else if (typeof(T) == typeof(GetActiveLOEventsRequest))
            {
                var requestData = (fakeMockupRequest as GetActiveLOEventsRequest).data;
                var fakeResponse = new ActiveLOEventsResponse();
                fakeResponse.Events = requestData.Events;
                foreach (var item in fakeResponse.Events)
                {
                    item.openedCount = GetEventOpenedCount(item.uniqueId);
                    item.openedAfterExpired = GetEventOpenedAfterExpired(item.uniqueId) == 1;
                }

                ReceiveMessage(fakeResponse);
            }
            
            PlayerPrefs.Save();
        }

        private async Task ReceiveMessage<T>(T message)
        {
            if (message == null) return;
            
            await Task.Delay(Random.Range(100,500));
            if (_subscribers.TryGetValue(typeof(T), out var del))
            {
                (del as Action<T>)?.Invoke(message);
            }
        }
        
        #region DebugPlayerPrefs
        //Values and methods just to simulate server behaviour
        private int GetEventOpenedCount(int uniqueEventId)
        {
            return PlayerPrefs.GetInt(GetEventOpenedCountPrefsKey(uniqueEventId), 0);
        }
        
        private void SaveEventOpenedCount(int uniqueEventId, int count)
        {
            PlayerPrefs.SetInt(GetEventOpenedCountPrefsKey(uniqueEventId), count);
        }
        
        private string GetEventOpenedCountPrefsKey(int uniqueEventId)
        {
            return EVENT_OPENED_COUNT_PREFIX_KEY + uniqueEventId;
        }
        
        private int GetEventOpenedAfterExpired(int uniqueEventId)
        {
            return PlayerPrefs.GetInt(GetEventOpenedAfterExpiredPrefsKey(uniqueEventId), 0);
        }
        
        private string GetEventOpenedAfterExpiredPrefsKey(int uniqueEventId)
        {
            return EVENT_OPENED_AFTER_EXPIRED_PREFIX_KEY + uniqueEventId;
        }
        
        private const string PLAYER_LEVEL_KEY = "PLAYER_LEVEL_KEY";
        private const string EVENT_OPENED_COUNT_PREFIX_KEY = "EVENT_OPENED_COUNT_KEY_";
        private const string EVENT_OPENED_AFTER_EXPIRED_PREFIX_KEY = "EVENT_OPENED_AFTER_EXPIRED_KEY_";
        #endregion
    }
}