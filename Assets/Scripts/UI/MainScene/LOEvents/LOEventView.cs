using TMPro;
using UI.Timer;
using UI.ViewControllerBase;
using UnityEngine;
using UnityEngine.UI;

namespace UI.MainScene.LOEvents
{
    public class LOEventView : ViewBase
    {
        [field: SerializeField] public TimerView Timer { get; private set; }
        [field: SerializeField] public Image Icon { get; private set; }
        [field: SerializeField] public Button Button { get; private set; }
        [field: SerializeField] public TextMeshProUGUI FinishedText { get; private set; }
    }
}