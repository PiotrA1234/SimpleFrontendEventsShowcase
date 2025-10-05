using TMPro;
using UI.ViewControllerBase;
using UnityEngine;
using UnityEngine.UI;

namespace UI.View
{
    public class MenuBottomView : ViewBase
    {
        [field: SerializeField] public TextMeshProUGUI LevelText { get; private set; }
        [field: SerializeField] public Button PlayButton { get; private set; }
    }
}