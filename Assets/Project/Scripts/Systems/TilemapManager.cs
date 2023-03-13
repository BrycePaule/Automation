using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : Singleton<TilemapManager>
{
    [Header("Settings")]
    [SerializeField] private int mapSize;

    [Header("Tiles")]
    [SerializeField] private TerrainTile BaseTile;
    [SerializeField] private TerrainTile AltBaseTile;
    [SerializeField] private TerrainTile Gem1Tile;
    [SerializeField] private TerrainTile Gem2Tile;
    
    [Header("References")]
    [SerializeField] private Tilemap tilemap;


    private Dictionary<Vector3Int, MapToken> tileCache = new Dictionary<Vector3Int, MapToken>();
    private Dictionary<Vector3Int, GameObject> buildingCache = new Dictionary<Vector3Int, GameObject>();

    private Vector3Int playerCellPos;

    // GENERATION

    public void SetTile(Vector2Int pos, MapToken token)
    {
        // if (token == MapToken.Ground) { tilemap.SetTile(pos, Instantiate(BaseTile)); }
        // if (token == MapToken.AlternateGround) { tilemap.SetTile(pos,Instantiate(AltBaseTile)); }
        // if (token == MapToken.Gem1) { tilemap.SetTile(pos,Instantiate(Gem1Tile)); }
        // if (token == MapToken.Gem2) { tilemap.SetTile(pos,Instantiate(Gem2Tile)); }

        if (token == MapToken.Ground) { tilemap.SetTile((Vector3Int) pos, BaseTile); }
        if (token == MapToken.AlternateGround) { tilemap.SetTile((Vector3Int) pos, AltBaseTile); }
        if (token == MapToken.Gem1) { tilemap.SetTile((Vector3Int) pos, Gem1Tile); }
        if (token == MapToken.Gem2) { tilemap.SetTile((Vector3Int) pos, Gem2Tile); }
    }

    public void RefreshTiles(Vector3Int playerPos)
    {
        playerCellPos = playerPos;

        foreach (var pos in Utils.EvaluateGrid(playerCellPos.x - (mapSize / 2), playerCellPos.y - (mapSize / 2), mapSize))
        {
            if (!tileCache.ContainsKey(pos))
            {
                tileCache[pos] = MapGenerator.Instance.GetTokenAtPos(pos, playerPos);
            }

            SetTile((Vector2Int) pos, tileCache[pos]);
        }
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
    
    public TerrainTile GetTile(Vector3Int cellPos)
    {
        return (TerrainTile) tilemap.GetTile(cellPos);
    }

    public void SetBuilding(Vector3Int cellPos, GameObject building)
    {
        buildingCache[cellPos] = building;
    }

    public void DestroyBuilding(Vector3Int cellPos)
    {
        if (buildingCache.ContainsKey(cellPos))
        {
            Destroy(buildingCache[cellPos]);
            buildingCache.Remove(cellPos);
        }
    }
    
    public GameObject GetBuilding(Vector3Int cellPos)
    {
        if (buildingCache.ContainsKey(cellPos))
        {
            return buildingCache[cellPos];
        }

        return null;
    }

    // CHECKERS

    public bool CanBuildAt(Vector3Int cellPos)
    {
        TerrainTile _tile = (TerrainTile) tilemap.GetTile(cellPos);

        if (!_tile.Buildable) { return false; }
        if (buildingCache.ContainsKey(cellPos)) { return false; }

        return true;
    }

    public bool CanPassAt(Vector3Int cellPos)
    {
        return ((TerrainTile) tilemap.GetTile(cellPos)).Passable;
    }

    public bool CanDrillAt(Vector3Int cellPos)
    {
        return ((TerrainTile) tilemap.GetTile(cellPos)).Drillable;
    }

    public bool InsideBounds(Vector3Int cellPos)
    {
        return tilemap.cellBounds.Contains(cellPos);
    }

    public bool ContainsBuilding(Vector3Int cellPos)
    {
        return buildingCache.ContainsKey(cellPos);
    }

    // GETTERS

}
