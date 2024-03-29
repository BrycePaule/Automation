using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace bpdev
{
    public class SaveManager : Singleton<SaveManager>
    {
        [Header("Current Savefile")]
        public scr_Savefile saveFile;
        
        [Header("Settings")]
        public bool SaveOnExit;
        public bool OverwriteSave;

        private void OnApplicationQuit()
        {
            if (SaveOnExit)
            {
                Save();
            }
        }


        // SAVE

        public void Save()
        {
            if (saveFile != null && OverwriteSave)
            {
                Debug.Log($"Saving over <{saveFile.name}>.");
            }
            else 
            {
                saveFile = GenerateNewSaveFile();
                Debug.Log($"Saved to new file <{saveFile.name}>");
            }

            SavePlayerPos();
            SaveMapData();
            EditorUtility.SetDirty(saveFile);
        }

        private void SavePlayerPos()
        {
            saveFile.PlayerCellPos = InputManager.Instance.PlayerCellPos;
        }

        private void SaveMapData()
        {
            saveFile.MapAsset.PerlinSettings = new List<PerlinSettings>();
            saveFile.MapAsset.PerlinSettings.Add(MapGenerator.Instance.BaseSettings);
            saveFile.MapAsset.PerlinSettings.Add(MapGenerator.Instance.Gem1Settings);
            saveFile.MapAsset.PerlinSettings.Add(MapGenerator.Instance.Gem2Settings);

            saveFile.MapAsset.BuildingCache = SaveBuildingData(TilemapManager.Instance.BuildingCache);
        }

        private List<BuildingInfo> SaveBuildingData(Dictionary<Vector3Int, GameObject> buildingsGOs)
        {
            List<BuildingInfo> buildingList = new List<BuildingInfo>();

            foreach (var building in buildingsGOs)
            {
                Building _bld = building.Value.GetComponent<Building>();

                buildingList.Add(new BuildingInfo(
                    _bld.BuildingType,
                    _bld.GetComponent<TSystemConnector>().Facing,
                    building.Key
                ));                
            }

            return buildingList;
        }


        // LOAD

        public void Load()
        {
            if (saveFile == null)
            {
                print("No file selected. Creating new world");
            }
            else
            {
                if (saveFile.MapAsset == null) { return; }

                print("Loading from save");

                Transform player = FindObjectOfType<PlayerMovement>().transform.parent;
                player.transform.position = TilemapManager.Instance.TileAnchorFromCellPos(saveFile.PlayerCellPos);

                MapGenerator.Instance.BaseSettings = saveFile.MapAsset.PerlinSettings[0];
                MapGenerator.Instance.Gem1Settings = saveFile.MapAsset.PerlinSettings[1];
                MapGenerator.Instance.Gem2Settings = saveFile.MapAsset.PerlinSettings[2];

                PlaceBuildings();

                MapGenerator.Instance.MapAsset = saveFile.MapAsset;
            }
        }

        private void PlaceBuildings()
        {
            foreach (BuildingInfo buildInfo in saveFile.MapAsset.BuildingCache)
            {
                BuildingProxy.Instance.InstantiateBuildingAt(buildInfo.Type, buildInfo.Location);
                TilemapManager.Instance.GetBuilding(buildInfo.Location)?.GetComponent<TSystemRotator>().RotateToFace(buildInfo.Direction);
            }
        }

        // SCRIPTABLES

        private scr_Savefile GenerateNewSaveFile()
        {
            scr_Savefile saveFile = scr_Savefile.CreateInstance<scr_Savefile>();
            saveFile.MapAsset = MapGenerator.Instance.CreateMapAsset();

            string PATH = AssetDatabase.GenerateUniqueAssetPath("Assets/_Project/ScriptableObjects/Saves/save_NEWSAVE.asset");
            AssetDatabase.CreateAsset(saveFile, PATH);

            return saveFile;
        }

    }
}