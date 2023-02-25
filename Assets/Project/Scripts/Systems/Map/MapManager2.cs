// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;

// public class MapManager2 : Singleton<MapManager>
// {
// 	public Tilemap Tilemap;
// 	public MapAsset Map;

// 	[Header("References")]
// 	public TilemapProxy TilemapProxy;
// 	public Transform TerrainContainer;
// 	public Transform BuildingsContainer;

// 	public enum ContainerType
// 	{
// 		Terrain,
// 		Buildings
// 	}

// 	// OBJECTS
// 	public void PlaceObject(Vector3Int pos, GameObject GO, ContainerType containerType)
// 	{
// 		switch (containerType)
// 		{
// 			case ContainerType.Terrain:
// 				GO.transform.SetParent(TerrainContainer);
// 				break;

// 			case ContainerType.Buildings:
// 				GO.transform.SetParent(BuildingsContainer);
// 				break;
// 		}

// 		if (!Map.TileObjectDict.ContainsKey(pos))
// 			Map.TileObjectDict[pos] = new List<GameObject>();
		
// 		Map.TileObjectDict[pos].Add(GO); 
// 	}

// 	public void RemoveObject(Vector3Int pos)
// 	{
// 		// public void RemoveObject(Vector3Int pos, GameObject go)
// 		// currently only stores 1 object, and is completely replaced
// 		if (!Map.TileObjectDict.ContainsKey(pos)) { return; }
// 		RemoveAllObjects(pos);
// 	}

// 	public void RemoveAllObjects(Vector3Int pos) => Map.TileObjectDict[pos] = new List<GameObject>();


// 	// TILES
// 	public MyTile GetTile(Vector3Int pos) => Map.TileDict[pos];

// 	public void SetTile(Vector3Int pos, TileType tileType)
// 	{
// 		MyTile tile = GetTileOfType(tileType);

// 		Tilemap.SetTile(pos, tile);
// 		Map.TileDict[pos] = tile;
// 	} 

// 	public void RemoveTile(Vector3Int pos)
// 	{
// 		Map.TileDict[pos] = null;
// 		Map.TileObjectDict[pos] = new List<GameObject>();
// 	}

// 	public bool IsTileOccupied(Vector3Int pos)
// 	{
// 		if (!Map.TileObjectDict.ContainsKey(pos)) { return false; }
// 		return Map.TileObjectDict[pos].Count > 0;
// 	}


// 	// HELPERS
// 	private MyTile GetTileOfType(TileType tileType)
// 	{
// 		return TileFactory.Instance.Create(tileType);
// 	}
// }
