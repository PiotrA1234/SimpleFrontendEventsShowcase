using UI.ViewControllerBase;
using UnityEngine;

namespace UI.MainScene.LOEvents
{
    public class LOEventsView : ViewBase
    {
        [field: SerializeField] public LOEventView[] LeftViews { get; private set; }
        [field: SerializeField] public LOEventView[] RightViews { get; private set; }
        public int AllViewsCount => LeftViews.Length + RightViews.Length;
    }
}