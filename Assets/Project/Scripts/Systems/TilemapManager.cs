using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : Singleton<TilemapManager>
{
    private Vector3 anchorOffset { get; }

    [Header("References")]
    [SerializeField] private Tilemap tilemap;

    [Header("Tiles")]
    [SerializeField] private MyTile BaseTile;
    [SerializeField] private MyTile AltBaseTile;
    [SerializeField] private MyTile Gem1Tile;
    [SerializeField] private MyTile Gem2Tile;

    // GENERATION
    public void SetTiles(MapToken[,] map, int mapSize)
    {
        for (int y = 0; y < mapSize; y++)
        {
            for (int x = 0; x < mapSize; x++)
            {
                int randOffset = -5;

                Vector3Int pos = new Vector3Int(y + randOffset, x + randOffset, 0);

                if (map[y, x] == MapToken.Ground) { SetTile(pos, Instantiate(BaseTile)); }
                if (map[y, x] == MapToken.AlternateGround) { SetTile(pos,Instantiate(AltBaseTile)); }
                if (map[y, x] == MapToken.Gem1) { SetTile(pos,Instantiate(Gem1Tile)); }
                if (map[y, x] == MapToken.Gem2) { SetTile(pos,Instantiate(Gem2Tile)); }
            }
        }
    }

    private void SetTile(Vector3Int pos, MyTile tile)
    {
        tilemap.SetTile(pos, tile);
    }


    // INTERACTIONS

    public Vector3Int WorldToCell(Vector3 worldPos)
    {
        return tilemap.WorldToCell(worldPos);
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
