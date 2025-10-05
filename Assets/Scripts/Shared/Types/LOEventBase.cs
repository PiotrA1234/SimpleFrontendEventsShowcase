using System;
using System.Linq;
using Converters;
using UnityEngine;

namespace Shared.Types
{
    [System.Serializable]
    public class LOEventBaseData
    {
        public int uniqueId;
        public string name;
        public int requiredLevel;
        [HideInInspector] public int openedCount;
        public long endTimestampInSeconds;
        public string addressableLabel;
        public StringStringKeyValue[] keysToAssets = Array.Empty<StringStringKeyValue>();
        [HideInInspector] public bool openedAfterExpired;
        
        public string ActiveSpriteAddress => keysToAssets.FirstOrDefault(kv => kv.key == "activeIcon")?.value;
        public string ExpiredSpriteAddress => keysToAssets.FirstOrDefault(kv => kv.key == "expiredIcon")?.value;
        public string PopupAddress => keysToAssets.FirstOrDefault(kv => kv.key == "popup")?.value;
        public bool Active => endTimestampInSeconds > TimeUtility.GetCurrentUTCTimestamp();
    }
}