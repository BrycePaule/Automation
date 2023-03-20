using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    [CreateAssetMenu(menuName="Scriptables/Map/Map Asset", fileName="New Map Asset")]
    public class scr_MapAsset : ScriptableObject
    {
        public List<PerlinSettings> PerlinSettings = new List<PerlinSettings>();

        public Dictionary<Vector3Int, int> TokenCache;
        public List<BuildingInfo> BuildingCache;
    }
}