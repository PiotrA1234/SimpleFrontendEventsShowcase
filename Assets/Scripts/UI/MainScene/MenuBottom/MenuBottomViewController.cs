using Player;
using UI.ViewControllerBase;

namespace UI.View
{
    public class MenuBottomViewController : ViewControllerBase<MenuBottomView>
    {
        private IPlayerDataManager _playerDataManager;

        public MenuBottomViewController(MenuBottomView view, IPlayerDataManager playerDataManager) : base(view)
        {
            _playerDataManager = playerDataManager;
            SetLevelText(_playerDataManager.Data.Level.Value);
            _playerDataManager.Data.Level.OnValueChanged += SetLevelText;
            _view.PlayButton.onClick.AddListener(OnPlayButtonClicked);
        }

        private void SetLevelText(int playerLevel)
        {
            _view.LevelText.text = PLAYED_COUNT_PREFIX + playerLevel;
        }
        
        private void OnPlayButtonClicked()
        {
           _playerDataManager.IncrementLevel();
        }
        
        private const string PLAYED_COUNT_PREFIX = "Played Counter:";
    }
}