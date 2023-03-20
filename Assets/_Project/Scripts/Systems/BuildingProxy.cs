using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class BuildingProxy : Singleton<BuildingProxy>
    {
        [SerializeField] private scr_BuildingLibrary library;

        // LIBRARY INTERFACING
        public scr_BuildingAsset GetAssetByID(int id)
        {
            return library.GetByID(id);
        }

        public scr_BuildingAsset GetAssetByType(BuildingType type)
        {
            return library.GetByType(type);
        }

        public GameObject InstantiateByType(BuildingType type)
        {
            if (type == BuildingType.UNASSIGNED) { Debug.LogError("BuildingProxy failed to Instatiate resource of Type: " + BuildingType.UNASSIGNED); }

            scr_BuildingAsset asset = GetAssetByType(type);

            return Instantiate(asset.Prefab, Vector3.zero, Quaternion.identity);
        }

        // CREATION, REMOVAL, INTERACTION

        public void InstantiateBuildingAt(BuildingType buildingType, Vector3Int cellPos)
        {
            if (buildingType == BuildingType.UNASSIGNED) { return; }
            if (!TilemapManager.Instance.CanBuildAt(cellPos)) { return; }

            GameObject _building = InstantiateByType(buildingType);
            _building.name = _building.name + " " + cellPos;
            _building.transform.position = TilemapManager.Instance.TileAnchorFromCellPos(cellPos);
            
            _building.GetComponent<ITSystemConnectable>().SetCellPosition(cellPos);

            TilemapManager.Instance.SetBuilding(cellPos, _building);
            TSystemManager.Instance.RefreshConnectionsAround(cellPos);
        }

        public void DestroyBuildingAt(Vector3Int cellPos)
        {
            TilemapManager.Instance.DestroyBuilding(cellPos);
            TSystemManager.Instance.RefreshConnectionsAround(cellPos);
        }

        public GameObject GetBuildingAt(Vector3Int cellPos)
        {
            return TilemapManager.Instance.GetBuilding(cellPos);
        }

    }
}