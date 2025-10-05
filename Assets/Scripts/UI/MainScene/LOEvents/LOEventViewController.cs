using System;
using System.Collections.Generic;
using System.Threading;
using Connection;
using Cysharp.Threading.Tasks;
using LOEvents;
using Popup;
using Shared;
using Shared.Types;
using UI.Manager;
using UI.Timer;
using UI.ViewControllerBase;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using Zenject;
using Object = UnityEngine.Object;

namespace UI.MainScene.LOEvents
{
    public class LOEventViewController : ViewControllerBase<LOEventView>, IDisposable
    {
        private LOEventBaseData _data = new();
        private TimerViewController _timerController;
        private AsyncOperationHandle<IList<Object>> _loadAssetsHandle;
        private AsyncOperationHandle<GameObject> _instantiatePopupHandle;
        private bool _loading;
        private Sprite _expiredSprite;
        private Sprite _activeSprite;
        private AssetReference _popupAssetReference;
        private PopupViewController<PopupView> _popupController;
        private CancellationTokenSource _cts = new();
        private DiContainer _container;
        private IUIManager _uiManager;
        private ILOEventsManager _eventsManager;
        private IConnectionManager _connectionManager;
        
        public LOEventBaseData Data => _data;
        public bool Loading => _loading;
        public bool Active => _view.gameObject.activeSelf;

        public LOEventViewController(LOEventView view, DiContainer container, IUIManager uiManager, ILOEventsManager eventsManager,
            IConnectionManager connectionManager) : base(view)
        {
            _container = container;
            _uiManager = uiManager;
            _eventsManager = eventsManager;
            _connectionManager = connectionManager;
            _connectionManager.Subscribe<OpenedExpiredEventResponse>(OnOpenedExpiredEventResponse);
            SetActivity(false);
            _timerController = container.Instantiate<TimerViewController>(new object[] { _view.Timer });
        }

        public void SetViewAsync(LOEventBaseData data)
        {
            if (data.Equals(_data)) return;
            
            _loading = true;
            _view.Button.onClick.RemoveListener(OpenPopup);
            _view.Button.onClick.AddListener(OpenPopup);
            _data = data;
            _cts?.Cancel();
            _cts = CancellationTokenSource.CreateLinkedTokenSource(_view.CancellationToken.Token);
            LoadAddressables().Forget();
        }

        public void Disable()
        {
            SetActivity(false);
            _data = new();
        }
        
        public override void Dispose()
        {
            if (_popupAssetReference != null && _popupAssetReference.IsValid())
            {
                _popupAssetReference.ReleaseAsset();
            }

            if (_loadAssetsHandle.IsValid())
            {
                Addressables.Release(_loadAssetsHandle);
            }

            if (_popupController != null)
            {
                _popupController.Dispose();
            }

            DisposePopupHandle();

            if (_timerController != null)
            {
                _timerController.Dispose();
            }
            
            _connectionManager.Unsubscribe<OpenedExpiredEventResponse>(OnOpenedExpiredEventResponse);
            base.Dispose();
        }

        
         private async UniTask LoadAddressables()
         {
            List<Object> loadedObjects = new();
            if (_loadAssetsHandle.IsValid())
            {
                Addressables.Release(_loadAssetsHandle);
            }

            _loadAssetsHandle = Addressables.LoadAssetsAsync<Object>(_data.addressableLabel, obj =>
            {
                loadedObjects.Add(obj);
            });

            await _loadAssetsHandle.ToUniTask(cancellationToken: _cts.Token);
            
            if (_cts.IsCancellationRequested)
            {
                _loadAssetsHandle.Release();
                _loading = false;
                return;
            }
            
            if (_loadAssetsHandle.Status == AsyncOperationStatus.Succeeded)
            {
                foreach (var obj in loadedObjects)
                {
                    if (obj.name == _data.ExpiredSpriteAddress && obj is Texture2D expiredTexture)
                    {
                        _expiredSprite = Sprite.Create(expiredTexture, new Rect(0, 0, expiredTexture.width, expiredTexture.height), new Vector2(0.5f, 0.5f));
                    }
                    else if(obj.name == _data.ActiveSpriteAddress && obj is Texture2D activeTexture)
                    {
                        _activeSprite = Sprite.Create(activeTexture, new Rect(0, 0, activeTexture.width, activeTexture.height), new Vector2(0.5f, 0.5f));
                    }
                    else if (obj.name == _data.PopupAddress && obj is GameObject)
                    {
                        _popupAssetReference = new AssetReference(_data.PopupAddress);
                    }
                    else
                    {
                        Debug.LogError($"Loaded object: {obj.name} does not match," +
                                       $" ExpiredSpriteAddress: {_data.ExpiredSpriteAddress}, ActiveSpriteAddress: {_data.ActiveSpriteAddress}, " +
                                       $"PopupAddress: {_data.PopupAddress}" + $"type: {obj.GetType()}", _view.gameObject);
                    }
                }
                SetView();
            }
            else
            {
                Debug.LogError($"Failed to load assets for label {_data.addressableLabel}, exception: {_loadAssetsHandle.OperationException}, " +
                               $"status: {_loadAssetsHandle.Status}, _data: {_data.ActiveSpriteAddress}");
            }
            _loadAssetsHandle.Release();
            _loading = false;
        }
        
        private void SetView()
        {
            bool active = _data.Active;
            
            _view.Icon.sprite = active ? _activeSprite : _expiredSprite;
            _view.FinishedText.gameObject.SetActive(!active);
            _timerController.SetActive(active);
            if (active)
            {
                _timerController.StartCountdown(_data.endTimestampInSeconds).SetOnFinished(OnTimerFinished);
            }

            SetActivity(true);
        }

        private void SetActivity(bool value)
        {
            _view.gameObject.SetActive(value);
        }

        private void OnTimerFinished()
        {
            _view.Icon.sprite = _expiredSprite;
        }

        private void OpenPopup()
        {
            DisposePopupHandle();

            _instantiatePopupHandle = Addressables.InstantiateAsync(_popupAssetReference, _uiManager.GetUIRoot);
            _popupController = _container.Instantiate<PopupViewController<PopupView>>
                (new object[] { _instantiatePopupHandle.WaitForCompletion().GetComponent<PopupView>(), _data, (Action)DisposePopupHandle});
            
            if (!_data.Active)
            {
                _eventsManager.OnOpenedExpiredEvent(_data);
                Disable();
            }
        }
        
        private void OnOpenedExpiredEventResponse(OpenedExpiredEventResponse response)
        {
            if (response.uniqueId == _data.uniqueId)
            {
                Disable();
            }
        }

        private void DisposePopupHandle()
        {
            if (_instantiatePopupHandle.IsValid())
            {
                Addressables.Release(_instantiatePopupHandle);
            }
        }
    }
}