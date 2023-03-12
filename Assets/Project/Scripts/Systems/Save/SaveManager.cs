using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
	[Header("Current Savefile")]
	public Savefile saveFile;
	public bool OverwriteSave;

	[Header("Save Resources")]
    [SerializeField] private GameObject Player;

	[Header("Load Resources")]
    [SerializeField] private scr_ResourceLibrary ResourceLibrary;
    [SerializeField] private scr_BuildingLibrary BuildingLibrary;

	[Header("References")]
    [SerializeField] private TilemapManager TilemapManager;


	private void OnApplicationQuit() => Save();

	public void Save()
    {
        if (saveFile == null)
        {
            Debug.Log("Unable to save, no save file selected.");
            return;
        }

        // if (!OverwriteSave)
        // {
        // 	Debug.Log("Exiting without saving.");
        // 	return;
        // }

        // SaveMapData();

        SavePlayerPos();

        EditorUtility.SetDirty(saveFile);
        Debug.Log("Saved");
    }

    private void SavePlayerPos()
    {
        saveFile.PlayerPos = Player.transform.position;
    }

	private void SaveMapData()
	{
		// OffGridMap _map = MapManager.Instance.Map;

		// for (int i = 0; i < _map.Size; i++)
		// {
		// 	for (int j = 0; j < _map.Size; j++)
		// 	{
		// 		Vector3Int _pos = new Vector3Int(i, j, 0);
				
		// 		// fetch objects
		// 		int objectCount = 0;
		// 		if (_map.TileObjectDict.ContainsKey(_pos))
		// 			objectCount = _map.TileObjectDict[_pos].Count;

		// 		int[] objectIDsToSave = new int[objectCount];

		// 		for (int n = 0; n < objectCount; n++)
		// 		{
		// 			if (_map.TileObjectDict[_pos][n].GetComponent<ObjectBase>() != null)
		// 			{
		// 				objectIDsToSave[n] = _map.TileObjectDict[_pos][n].GetComponent<ObjectBase>().ObjectID;

		// 				// this break is only here temporarily to cap objects at one per tile - for now...
		// 				break;
		// 			}
		// 		}

		// 		// fetch tiletype
		// 		TileType tileTypeToSave = MapManager.Instance.GetTile(_pos).TileType;

		// 		// fetch location
		// 		Vector3Int locationToSave = new Vector3Int(i, j, 0);

		// 		saveFile.TileInfo[i * _map.Size + j] = new TileInfo(locationToSave, tileTypeToSave, objectIDsToSave);
		// 	}
		// }
	}

	public void Load()
	{
		// if (saveFile == null)
		// {
		// 	MapGenerator.GenerateNewMap();
		// }
		// else
		// {
		// 	print("Loading from save");
		// 	Player.GetComponentInChildren<InventoryProxy>().Inventory = saveFile.PlayerInventory;
		// 	CraftingRecipeManager.PlayerItemsCollected = saveFile.PlayerItemsCollected;
		// 	CraftingRecipeManager.PlayerRecipesUnlocked = saveFile.PlayerRecipesUnlocked;
		// }
	}
}