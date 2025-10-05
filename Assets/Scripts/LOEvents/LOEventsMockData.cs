using Shared.Types;
using UnityEngine;

namespace LOEvents
{
    [CreateAssetMenu(fileName = "LOEventsMockData", menuName = "ScriptableObjects/LOEventsMockData", order = 1)]
    public class LOEventsMockData : ScriptableObject
    {
        public LOEventBaseData[] Events;
    }
}