using System;
using System.Linq;
using LOEvents;
using Player;
using Shared.Types;
using UI.ViewControllerBase;
using Zenject;

namespace UI.MainScene.LOEvents
{
    public class LOEventsViewController : ViewControllerBase<LOEventsView>
    {
        private LOEventViewController[] _activeEvents = Array.Empty<LOEventViewController>();
        private IPlayerDataManager _playerDataManager;
        private LOEventBaseData[] _currentData;
        
        public LOEventsViewController(LOEventsView view, ILOEventsManager eventsManager, DiContainer container,
            IPlayerDataManager playerDataManager) : base(view)
        {
            _playerDataManager = playerDataManager;
            _activeEvents = new LOEventViewController[_view.AllViewsCount];
            
            int index = 0;
            foreach (var leftView in _view.LeftViews)
            {
                _activeEvents[index++] = container.Instantiate<LOEventViewController>(new object[] { leftView });
            }
            foreach (var rightview in _view.RightViews)
            {
                _activeEvents[index++] = container.Instantiate<LOEventViewController>(new object[] { rightview });
            }

            SetView(eventsManager.Data.Value);
            eventsManager.Data.OnValueChanged += SetView;
            playerDataManager.Data.Level.OnValueChanged += OnPlayerLevelChanged;
        }

        private void OnPlayerLevelChanged(int level)
        {
            if (_currentData != null)
            {
                SetView(_currentData);
            }
        }

        private void SetView(LOEventBaseData[] data)
        {
            _currentData = data;

            foreach (var activeEvent in _activeEvents)
            {
                if (activeEvent.Active)
                {
                    if (!data.Any(e => e.uniqueId == activeEvent.Data.uniqueId))
                    {
                        activeEvent.Disable();
                    }
                }
            }

            foreach (var eventData in data.Take(_view.AllViewsCount))
            {
                if (eventData.openedAfterExpired)
                {
                    continue;
                }

                if (eventData.requiredLevel > _playerDataManager.Data.Level.Value)
                {
                    continue;
                }

                var alreadyActive = _activeEvents.FirstOrDefault(x => x.Data.uniqueId == eventData.uniqueId);
                
                if (alreadyActive == default)
                {
                    alreadyActive = _activeEvents.FirstOrDefault(x => !x.Loading && !x.Active);
                }
                
                alreadyActive.SetViewAsync(eventData);
            }
        }
    }
}