using UI.MainScene.LOEvents;
using UI.View;
using UI.ViewControllerBase;
using UnityEngine;

namespace UI.MainScene
{
    public class MainSceneView : ViewBase
    {
        [field: SerializeField] public MenuBottomView MenuBottomView;
        [field: SerializeField] public LOEventsView LOEventsView;
    }
}