using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : Singleton<TilemapManager>
{
    private Vector3 anchorOffset { get; }

    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    public bool IsLayerAtWorldPos(Layers layer, Vector3 worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(worldPos, Vector2.zero);
        return (_hit && _hit.transform.gameObject.layer == (int) layer);
    }

    public Vector3 TileAnchorFromWorldPos(Vector3 worldPos)
    {
        Vector3Int _cellPos = tilemap.WorldToCell(worldPos);
        // Vector3 _baseTilemapOffset = new Vector3(.5f, .5f, 0);

        return tilemap.CellToWorld(_cellPos) + tilemap.tileAnchor;
    }

    public Vector3 TileAnchorFromCellPos(Vector3Int cellPos)
    {
        return tilemap.CellToWorld(cellPos) + tilemap.tileAnchor;
    }

    public Vector3Int CellFromWorldPos(Vector3 worldPos)
    {
        return tilemap.WorldToCell(worldPos);
    }
    
    public MyTile GetTile(Vector3Int cellPos)
    {
        return (MyTile) tilemap.GetTile(cellPos);
    }

    public void DestroyBuilding(Vector3Int cellPos)
    {
        MyTile _tile = GetTile(cellPos);

        if (_tile.Building == null) { return; }

        Destroy(_tile.Building);
        _tile.Building = null;
    }
    
    // CHECKERS

    public bool CanBuildAt(Vector3Int cellPos)
    {
        MyTile _tile = (MyTile) tilemap.GetTile(cellPos);

        if (!_tile.Buildable) { return false; }
        if (_tile.Building != null) { return false; }

        return true;
    }

    public bool CanPassAt(Vector3Int cellPos)
    {
        return ((MyTile) tilemap.GetTile(cellPos)).Passable;
    }

    public bool CanDrillAt(Vector3Int cellPos)
    {
        return ((MyTile) tilemap.GetTile(cellPos)).Drillable;
    }

    public bool InsideBounds(Vector3Int cellPos)
    {
        return tilemap.cellBounds.Contains(cellPos);
    }
}
