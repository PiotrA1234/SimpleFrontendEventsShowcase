using System;
using Connection;
using LOEvents;
using Shared;
using Shared.Types;
using UI.ViewControllerBase;

namespace Popup
{
    public class PopupViewController<T> : ViewControllerBase<T> where T: PopupView
    {
        private event Action _onDispose;
        private LOEventBaseData _eventData;
        
        public PopupViewController(T view, LOEventBaseData eventData, Action onDispose, ILOEventsManager eventsManager,
            IConnectionManager connectionManager) : base(view)
        {
            _eventData = eventData;
            _onDispose += onDispose;
            connectionManager.Subscribe<IncrementEventOpenedCountResponse>(OnIncrementEventOpenedCountResponse);
            eventsManager.OnEventOpened(_eventData);
            _view.CounterText.text = _eventData.openedCount.ToString();
            _view.CloseButton.onClick.AddListener(Dispose);
        }

        public override void Dispose()
        {
            _onDispose?.Invoke();
            base.Dispose();
        }
        
        private void OnIncrementEventOpenedCountResponse(IncrementEventOpenedCountResponse response)
        {
            //if condition for the case of network lag, because messages can arrive out of order
            if (response.openedCount >= _eventData.openedCount)
            {
                _view.CounterText.text = response.openedCount.ToString();
            }
        }
    }
}