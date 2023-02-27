// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using UnityEditor;

// public class MapGenerator2 : MonoBehaviour
// {
// 	[Header("References")]
// 	public InputManager InputManager;
// 	public MapManager MapManager;
// 	public Tilemap Tilemap;
// 	public Transform TerrainContainer;
// 	public ObjectLibrary ObjectLibrary;
// 	public GameObject NorthWall;
// 	public GameObject SouthWall;
// 	public GameObject EastWall;
// 	public GameObject WestWall;

// 	[Header("Settings")]
// 	public int TilemapSize;

// 	public List<Spawnable> Spawnables;
// 	[HideInInspector] public MapAsset Map;

// 	private TerrainGenerator terrainGenerator;

// 	private void Awake()
// 	{
// 		terrainGenerator = GetComponentInChildren<TerrainGenerator>();
// 	}

// 	private void Start()
// 	{
// 		SetTilemapWalls();
// 	}

// 	// MAP GENERATION

// 	// public void GenerateNewMap(Savefile savefile = null)
// 	// {
// 	// 	CreateNewMapObject();

// 	// 	SetTiles(terrainGenerator.GenerateTexture());
// 	// 	SpawnRound();
// 	// }

// 	// public void LoadMapFromSave(Savefile file)
// 	// {
// 	// 	CreateNewMapObject();

// 	// 	foreach (TileInfo info in file.TileInfo)
// 	// 	{
// 	// 		MapManager.Instance.SetTile(info.TileLocation, info.TileType);
// 	// 	}

// 	// 	SpawnRoundFromSaveFile(file);
// 	// }

// 	private void CreateNewMapObject()
// 	{
// 		Map = MapAsset.CreateInstance<MapAsset>();
// 		MapManager.Instance.Map = Map;
// 		MapManager.Instance.Tilemap = Tilemap;
// 		InputManager.Map = Map;

// 		string _uniqueFilename = AssetDatabase.GenerateUniqueAssetPath("Assets/Project/Runtime/ScriptableObjects/Map/Maps/New Map.asset");
// 		AssetDatabase.CreateAsset(Map, _uniqueFilename);

// 		Map.Size = TilemapSize;
// 	}

// 	// TILEMAP SETUP

// 	private void SetTilemapWalls()
// 	{
				
// 		// Walls start as small boxes, get scaled to the size of the tilemap,
// 		// and placed alongside the edges of placed tiles

// 		Vector3 _horizontalScale = new Vector3(TilemapSize * 1.2f, 1, 0);
// 		Vector3 _verticalScale = new Vector3(1, TilemapSize * 1.2f, 0);

// 		NorthWall.transform.localScale = _horizontalScale;
// 		NorthWall.transform.position = new Vector3(-(Mathf.Abs(TilemapSize - _horizontalScale.x) / 2), TilemapSize, 0);

// 		SouthWall.transform.localScale = _horizontalScale;
// 		SouthWall.transform.position = new Vector3(-(Mathf.Abs(TilemapSize - _horizontalScale.x) / 2), -1, 0);

// 		EastWall.transform.localScale = _verticalScale;
// 		EastWall.transform.position = new Vector3(TilemapSize, -(Mathf.Abs(TilemapSize - _verticalScale.y) / 2), 0);

// 		WestWall.transform.localScale = _verticalScale;
// 		WestWall.transform.position = new Vector3(-1, -(Mathf.Abs(TilemapSize - _verticalScale.y) / 2), 0);
// 	}

// 	private void SetTiles(Texture2D texture)
// 	{
// 		for (int i = 0; i < TilemapSize; i++)
// 		{
// 			for (int j = 0; j < TilemapSize; j++)
// 			{
// 				Vector3Int _pos = new Vector3Int(i, j, 0);

// 				if (texture.GetPixel(i, j) == Color.white) 
// 				{
// 					MapManager.Instance.SetTile(_pos, TileType.Grass);
// 					// if (OffGridUtils.ChanceRoll(.1f))
// 					// 	MapManager.Instance.SetTile(_pos, TileType.Dirt);
// 				}

// 				if (texture.GetPixel(i, j) == Color.red) 
// 				{
// 					MapManager.Instance.SetTile(_pos, TileType.Dirt);
// 				}

// 				if (texture.GetPixel(i, j) == Color.green) 
// 				{
// 					MapManager.Instance.SetTile(_pos, TileType.Undergrowth);
// 				}

// 			}
// 		}
// 	}

// 	// OBJECT SETUP

// 	public void SpawnRoundFromSaveFile(Savefile saveFile)
// 	{
// 		for (int i = 0; i < TilemapSize; i++)
// 		{
// 			for (int j = 0; j < TilemapSize; j++)
// 			{
// 				TileInfo info = saveFile.TileInfo[i * saveFile.MapSize + j];

// 				// spawn objects in TileInfo
// 				foreach (int ID in info.ObjectIDs)
// 				{
// 					GameObject GO = ObjectFactory.Instance.SpawnObjectOfID(ID, info.TileLocation);
// 					MapManager.Instance.PlaceObject(info.TileLocation, GO, MapManager.ContainerType.Terrain);
// 				}
// 			}
// 		}
// 	}

// 	public void SpawnRound(float chanceForEachTileToSpawn = 1f)
// 	{
// 		for (int i = 0; i < TilemapSize; i++)
// 		{
// 			for (int j = 0; j < TilemapSize; j++)
// 			{
// 				Vector3Int _pos = new Vector3Int(i, j, 0);

// 				if (!OffGridUtils.ChanceRoll(chanceForEachTileToSpawn)) { continue; }
// 				if (MapManager.Instance.IsTileOccupied(_pos)) { continue; }

// 				foreach (Spawnable spawnable in Spawnables)
// 				{
// 					// check if object can spawn on current tile
// 					if (!spawnable.SpawnableOn.Contains(MapManager.GetTile(_pos).TileType))
// 					{
// 						continue;
// 					}

// 					// chance to spawn
// 					if (OffGridUtils.ChanceRoll(spawnable.SpawnChance))
// 					{
// 						Spawn(spawnable, _pos);
// 						if (spawnable.Exclusive) { break; }
// 					}
// 				}
// 			}
// 		}
// 	}

// 	private void Spawn(Spawnable spawnable, Vector3Int location)
// 	{
// 		GameObject GO = ObjectFactory.Instance.SpawnObjectOfID(spawnable.Object.ObjectID, location);
		
// 		if (spawnable.RandomiseSprites)
// 		{
// 			SpriteRenderer _sr = GO.GetComponent<SpriteRenderer>();
// 			_sr.sprite = spawnable.Sprites[Random.Range(0, spawnable.Sprites.Count)];

// 			// this should maybe be removed, if shadows are drawn manually on sprites
// 			// flipping will put shadows on the wrong side
// 			if (OffGridUtils.ChanceRoll(0.5f))
// 			{
// 				if (GO.transform.position.x != 0 & !MapManager.Instance.IsTileOccupied(location))
// 				{
// 					_sr.flipX = true;
// 					GO.transform.position = new Vector3(GO.transform.position.x + 1, GO.transform.position.y, GO.transform.position.z);
// 					location.x += 1;
// 				}
// 			} 
// 		}

// 		MapManager.Instance.PlaceObject(location, GO, MapManager.ContainerType.Terrain);
// 	}

// }
