using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

[System.Serializable]
[CreateAssetMenu(menuName="Scriptables/Map/Map", fileName="New Map")]
public class MapAsset : ScriptableObject
{
	public Tilemap Tilemap;
	public int Size;
	public Dictionary<Vector3Int, MyTile> TileDict = new Dictionary<Vector3Int, MyTile>();
	public Dictionary<Vector3Int, List<GameObject>> TileObjectDict = new Dictionary<Vector3Int, List<GameObject>>();
}
