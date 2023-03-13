using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class SaveManager : MonoBehaviour
{
    [Header("Current Savefile")]
    public scr_Savefile saveFile;
    public bool OverwriteSave;

    [Header("Save Resources")]
    [SerializeField] private GameObject Player;

    [Header("Load Resources")]
    [SerializeField] private scr_ResourceLibrary ResourceLibrary;
    [SerializeField] private scr_BuildingLibrary BuildingLibrary;

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

        SavePlayerPos();
        SaveMapData();

        EditorUtility.SetDirty(saveFile);
        Debug.Log("Saved");
    }

    private void SavePlayerPos()
    {
        saveFile.PlayerCellPos = TilemapManager.Instance.WorldToCell(Player.transform.position);
    }

    private void SaveMapData()
    {
        saveFile.MapAsset.TokenCache = TilemapManager.Instance.TokenCache;
        saveFile.MapAsset.BuildingCache = TilemapManager.Instance.BuildingCache;
    }

    public void Load()
    {

        if (saveFile == null)
        {
            // 
        }
        else
        {
            print("Loading from save");
            Player.transform.position = TilemapManager.Instance.TileAnchorFromCellPos(saveFile.PlayerCellPos);

            TilemapManager.Instance.TokenCache = saveFile.MapAsset.TokenCache;
            TilemapManager.Instance.BuildingCache = saveFile.MapAsset.BuildingCache;
        }
    }
}