using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TilemapManager : MonoBehaviour
{

    [Header("References")]
    [SerializeField] private Tilemap _tilemap;


    public bool IsLayerAtWorldPos(Layers _layer, Vector3 _worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.up);
        return (_hit && _hit.transform.gameObject.layer == (int) _layer);
    }

    public Vector3 TileAnchorFromWorldPos(Vector3 _worldPos)
    {
        Vector3Int _cellPos = _tilemap.WorldToCell(_worldPos);
        Vector3 _baseTilemapOffset = new Vector3(.5f, .5f, 0);

        return _tilemap.CellToWorld(_cellPos) + _baseTilemapOffset;
    }
}
