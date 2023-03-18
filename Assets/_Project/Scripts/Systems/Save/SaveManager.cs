using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

namespace bpdev
{
    public class SaveManager : Singleton<SaveManager>
    {
        [Header("Current Savefile")]
        public scr_Savefile saveFile;
        public bool OverwriteSave;

        // [Header("Save Resources")]

        [Header("Load Resources")]
        [SerializeField] private scr_ResourceLibrary ResourceLibrary;
        [SerializeField] private scr_BuildingLibrary BuildingLibrary;

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnApplicationQuit() => Save();

        public void Save()
        {
            if (saveFile == null)
            {
                Debug.Log("Unable to save, no save file selected.");
                return;
            }

            SavePlayerPos();
            SaveMapData();

            EditorUtility.SetDirty(saveFile);
            Debug.Log("Saved");
        }

        private void SavePlayerPos()
        {
            Transform player = FindObjectOfType<PlayerMovement>().transform.parent;
            saveFile.PlayerCellPos = TilemapManager.Instance.WorldToCell(player.position);
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
                print("No file selected. Creating new world");
            }
            else
            {
                print("Loading from save");
                Transform player = FindObjectOfType<PlayerMovement>().transform.parent;
                player.transform.position = TilemapManager.Instance.TileAnchorFromCellPos(saveFile.PlayerCellPos);

                TilemapManager.Instance.TokenCache = saveFile.MapAsset.TokenCache;
                TilemapManager.Instance.BuildingCache = saveFile.MapAsset.BuildingCache;
            }
        }
    }
}