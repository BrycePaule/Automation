using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    public bool IsLayerAtWorldPos(Layers _layer, Vector3 _worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.zero);
        return (_hit && _hit.transform.gameObject.layer == (int) _layer);
    }

    public Vector3 TileAnchorFromWorldPos(Vector3 _worldPos)
    {
        Vector3Int _cellPos = tilemap.WorldToCell(_worldPos);
        // Vector3 _baseTilemapOffset = new Vector3(.5f, .5f, 0);

        return tilemap.CellToWorld(_cellPos) + tilemap.tileAnchor;
    }

    public Vector3Int CellFromWorldPos(Vector3 worldPos)
    {
        return tilemap.WorldToCell(worldPos);
    }
    
    // CHECKERS

    public bool CanBuildAt(Vector3 worldPos)
    {
        Vector3Int _cellPos = CellFromWorldPos(worldPos);
        MyTile _tile = (MyTile) tilemap.GetTile(_cellPos);

        return _tile.Buildable;
    }

    public bool CanPassAt(Vector3 worldPos)
    {
        Vector3Int _cellPos = CellFromWorldPos(worldPos);
        MyTile _tile = (MyTile) tilemap.GetTile(_cellPos);

        return _tile.Passable;
    }

    public bool CanDrillAt(Vector3 worldPos)
    {
        Vector3Int _cellPos = CellFromWorldPos(worldPos);
        MyTile _tile = (MyTile) tilemap.GetTile(_cellPos);

        return _tile.Drillable;
    }
}
