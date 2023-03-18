using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace bpdev
{
    [System.Serializable]
    [CreateAssetMenu(menuName="Scriptables/Map/Map Asset", fileName="New Map Asset")]
    public class scr_MapAsset : ScriptableObject
    {
        public List<PerlinSettings> PerlinSettings = new List<PerlinSettings>();

        public Dictionary<Vector3Int, MapToken> TokenCache;
        public Dictionary<Vector3Int, GameObject> BuildingCache;
    }
}