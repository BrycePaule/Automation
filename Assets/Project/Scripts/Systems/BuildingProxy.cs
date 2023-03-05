using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

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

        MyTile _tile = TilemapManager.Instance.GetTile(cellPos);
        _tile.Building = _building;

        TSystemManager.Instance.RefreshConnectionsAround(cellPos);
    }

    public void DestroyBuildingAt(Vector3Int cellPos)
    {
        GameObject _building = GetBuildingAt(cellPos);

        if (_building != null)
        {
            TilemapManager.Instance.DestroyBuilding(cellPos);
            TSystemManager.Instance.RefreshConnectionsAround(cellPos);
        }
    }

    public GameObject GetBuildingAt(Vector3Int cellPos)
    {
        GameObject _building = TilemapManager.Instance.GetTile(cellPos).Building;

        if (_building == null) { return null; }

        return _building;
    }

}
