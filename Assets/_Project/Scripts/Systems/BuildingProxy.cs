using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class BuildingProxy : Singleton<BuildingProxy>
    {
        [SerializeField] private scr_BuildingLibrary library;


        // LIBRARY INTERFACE (fetching assets)

        public scr_BuildingAsset GetAssetByID(int id)
        {
            return library.GetByID(id);
        }

        public scr_BuildingAsset GetAssetByType(BuildingType type)
        {
            return library.GetByType(type);
        }


        // CREATION

        public GameObject InstantiateBuildingPrefab(BuildingType type)
        {
            if (type == BuildingType.UNASSIGNED) { Debug.LogError("BuildingProxy failed to Instatiate building of Type: " + BuildingType.UNASSIGNED); }

            scr_BuildingAsset asset = GetAssetByType(type);

            return Instantiate(asset.Prefab, Vector3.zero, Quaternion.identity);
        }

        public void InstantiateBuildingAt(BuildingType buildingType, Vector3Int cellPos)
        {
            if (buildingType == BuildingType.UNASSIGNED) { return; }
            if (!TilemapManager.Instance.CanBuildAt(cellPos)) { return; }

            GameObject _building = InstantiateBuildingPrefab(buildingType);
            _building.name = _building.name + " " + cellPos;
            _building.transform.position = TilemapManager.Instance.TileAnchorFromCellPos(cellPos);
            
            _building.GetComponent<ITSystemConnectable>().SetCellPosition(cellPos);
            _building.GetComponent<ITSystemRotatable>().RotateToFace(TileCursor.Instance.Direction);

            TilemapManager.Instance.SetBuilding(cellPos, _building);
            TSystemManager.Instance.RefreshConnectionsAround(cellPos);
        }


        // INTERACTION & DESTRUCTION

        public GameObject GetBuildingAt(Vector3Int cellPos)
        {
            return TilemapManager.Instance.GetBuilding(cellPos);
        }

        public void DestroyBuildingAt(Vector3Int cellPos)
        {
            TilemapManager.Instance.DestroyBuilding(cellPos);
            TSystemManager.Instance.RefreshConnectionsAround(cellPos);
        }
    }
}