// using System;
// using System.Collections;
// using System.Collections.Generic;
// using UnityEngine;
// using UnityEngine.Tilemaps;
// using UnityEditor;

// public class SaveManager : MonoBehaviour
// {
// 	[Header("Current Savefile")]
// 	public Savefile saveFile;
// 	public bool OverwriteSave;

// 	[Header("Save Resources")]
// 	public GameObject Player;
// 	public MapGenerator MapGenerator;
// 	public MapManager MapManager;
// 	public TimeManager TimeManager;
// 	public CraftingRecipeManager CraftingRecipeManager;

// 	[Header("Load Resources")]
// 	public ItemLibrary ItemLibrary;
// 	public ObjectLibrary ObjectLibrary;

// 	private void OnApplicationQuit() => Save();

// 	public void Save()
// 	{
// 		if (saveFile == null)
// 		{
// 			Debug.Log("Unable to save, no save file selected.");
// 			return;
// 		}

// 		if (!OverwriteSave)
// 		{
// 			Debug.Log("Exiting without saving.");
// 			return;
// 		}

// 		SavePlayerInventoryData();
// 		SaveMapData();

// 		saveFile.PlayerPos = Player.transform.position;

// 		saveFile.TimeDay = TimeManager.Day;
// 		saveFile.TimeHour = TimeManager.Hour;
// 		saveFile.TimeMinute = TimeManager.Minute;

// 		EditorUtility.SetDirty(saveFile);

// 		Debug.Log("Saved");
// 	}

// 	private void SavePlayerInventoryData()
// 	{
// 		saveFile.PlayerInventory = Player.GetComponentInChildren<InventoryProxy>().Inventory;
// 		saveFile.PlayerItemsCollected = CraftingRecipeManager.PlayerItemsCollected;
// 		saveFile.PlayerRecipesUnlocked = CraftingRecipeManager.PlayerRecipesUnlocked;
// 	}

// 	private void SaveMapData()
// 	{
// 		OffGridMap _map = MapManager.Instance.Map;

// 		for (int i = 0; i < _map.Size; i++)
// 		{
// 			for (int j = 0; j < _map.Size; j++)
// 			{
// 				Vector3Int _pos = new Vector3Int(i, j, 0);
				
// 				// fetch objects
// 				int objectCount = 0;
// 				if (_map.TileObjectDict.ContainsKey(_pos))
// 					objectCount = _map.TileObjectDict[_pos].Count;

// 				int[] objectIDsToSave = new int[objectCount];

// 				for (int n = 0; n < objectCount; n++)
// 				{
// 					if (_map.TileObjectDict[_pos][n].GetComponent<ObjectBase>() != null)
// 					{
// 						objectIDsToSave[n] = _map.TileObjectDict[_pos][n].GetComponent<ObjectBase>().ObjectID;

// 						// this break is only here temporarily to cap objects at one per tile - for now...
// 						break;
// 					}
// 				}

// 				// fetch tiletype
// 				TileType tileTypeToSave = MapManager.Instance.GetTile(_pos).TileType;

// 				// fetch location
// 				Vector3Int locationToSave = new Vector3Int(i, j, 0);

// 				saveFile.TileInfo[i * _map.Size + j] = new TileInfo(locationToSave, tileTypeToSave, objectIDsToSave);
// 			}
// 		}
// 	}

// 	public void Load()
// 	{
// 		if (saveFile == null)
// 		{
// 			MapGenerator.GenerateNewMap();
// 			TimeManager.SetDateTime(
// 				new DateTime()
// 				.AddDays(0)
// 				.AddHours(6)
// 				.AddMinutes(0));
// 		}
// 		else
// 		{
// 			print("Loading from save");
// 			Player.GetComponentInChildren<InventoryProxy>().Inventory = saveFile.PlayerInventory;
// 			CraftingRecipeManager.PlayerItemsCollected = saveFile.PlayerItemsCollected;
// 			CraftingRecipeManager.PlayerRecipesUnlocked = saveFile.PlayerRecipesUnlocked;

// 			MapGenerator.LoadMapFromSave(saveFile);

// 			Player.transform.position = saveFile.PlayerPos;

// 			TimeManager.SetDateTime(
// 				new DateTime()
// 				.AddDays(saveFile.TimeDay)
// 				.AddHours(saveFile.TimeHour)
// 				.AddMinutes(saveFile.TimeMinute));
// 		}
// 	}
// }