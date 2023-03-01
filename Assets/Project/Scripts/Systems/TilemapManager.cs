using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    // [SerializeField] private Tile _baseTile;
    // [SerializeField] private Tile _rockTile;

    private void Awake()
    {
        // map = new int[size,size];

        // SetMap();
        // CullRocks();

        // SetTiles();
    }

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

    // private void SetTiles()
    // {
    //     for (int y = 0; y < size; y++)
    //     {
    //         for (int x = 0; x < size; x++)
    //         {
    //             Vector3Int pos = new Vector3Int(y, x, 0);

    //             if (map[y, x] == 1)
    //             {
    //                 _tilemap.SetTile(pos, _rockTile);
    //             }

    //             if (map[y, x] == 0)
    //             {
    //                 _tilemap.SetTile(pos, _baseTile);
    //             }
    //         }
    //     }
    // }

    // private void SetMap()
    // {
    //     int _radius = 5;

    //     for (int i = 0; i < size; i++)
    //     {
    //         for (int j = 0; j < size; j++)
    //         {
    //             if (Utils.Roll(0.05f)) // ROCK
    //             {
    //                 for (int a = (i-_radius); a <=  (i+_radius); a++)
    //                 {
    //                     for (int b = (j-_radius); b <= (j+_radius); b++)
    //                     {
    //                         if (a < 0 || a >= size) { continue; }
    //                         if (b < 0 || b >= size) { continue; }

    //                         map[a, b] = 1;
    //                     }
    //                 }
    //             }
    //             else // BASE TILE
    //             {
    //                 map[i, j] = 0;
    //             }
    //         }
    //     }
    // }

    // private void CullRocks()
    // {
    //     for (int i = 0; i < size; i++)
    //     {
    //         for (int j = 0; j < size; j++)
    //         {
    //             if (map[i, j] == 1)
    //             {
    //                 if (Utils.Roll(25f))
    //                 {
    //                     map[i, j] = 0;
    //                 }
    //             }
    //         }
    //     }

    // }

}
