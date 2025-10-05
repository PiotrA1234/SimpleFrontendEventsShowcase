using TMPro;
using UI.ViewControllerBase;
using UnityEngine;
using UnityEngine.UI;

namespace Popup
{
    public class PopupView : ViewBase
    {
        [field: SerializeField] public Button CloseButton { get; private set; }
        [field: SerializeField] public TextMeshProUGUI CounterText { get; private set; }
    }
}