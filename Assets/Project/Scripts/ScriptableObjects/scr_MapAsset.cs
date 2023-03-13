using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName="Scriptables/Map/Map Asset", fileName="New Map Asset")]
public class scr_MapAsset : ScriptableObject
{
    public List<PerlinSettings> PerlinSettings = new List<PerlinSettings>();
	public Dictionary<Vector3Int, GameObject> Objects = new Dictionary<Vector3Int, GameObject>();
}
