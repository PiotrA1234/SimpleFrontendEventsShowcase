using UI.MainScene.LOEvents;
using UI.View;
using UI.ViewControllerBase;
using Zenject;

namespace UI.MainScene
{
    public class MainSceneViewController : ViewControllerBase<MainSceneView>
    {
        private MenuBottomViewController _menuBottomViewController;
        private LOEventsViewController _loEventsViewController;
        
        public MainSceneViewController(MainSceneView view, DiContainer container) : base(view)
        {
            _menuBottomViewController = container.Instantiate<MenuBottomViewController>(new object[] { _view.MenuBottomView });
            _loEventsViewController = container.Instantiate<LOEventsViewController>(new object[] { _view.LOEventsView });
        }
    }
}