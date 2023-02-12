using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private GameObject conveyerPrefab;
    [SerializeField] private GameObject markerPrefab;


    // CREATION, REMOVAL, INTERACTION

    public Conveyor CreateConveyorAtWorldPos(Vector3 _worldPos)
    {
        Vector3Int _cellPos = _tilemap.WorldToCell(_worldPos);

        GameObject _convObj = Instantiate(conveyerPrefab, WorldPosCentredOnTile(_worldPos), Quaternion.identity);
        Conveyor _conv = _convObj.GetComponent<Conveyor>();
        _conv.name = "Conveyor: " + _cellPos;
        _conv.SetReferences(this, _cellPos);

        RefreshConveyorConnectionsAroundWorldPos(_worldPos);

        return _conv;
    }

    public void DestroyConveyorAtWorldPos(Vector3 _worldPos)
    {
        Conveyor _conv = GetConveyorAtWorldPos(_worldPos);

        if (_conv)
        {
            Destroy(_conv.gameObject);
            RefreshConveyorConnectionsAroundWorldPos(_worldPos);
        }
    }

    public Conveyor GetConveyorAtWorldPos(Vector3 _worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.up);

        if (_hit && _hit.transform.gameObject.layer == (int) Layers.Conveyer)
        {
            return _hit.transform.GetComponent<Conveyor>();
        }
        
        return null;
    }

    public Conveyor GetConveyorAtScreenPos(Vector2 _screenPos)
    {
        Vector3 _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);
        return GetConveyorAtWorldPos(_worldPos);
    }

    // HELPERS
    
    public bool IsLayerAtWorldPos(Layers _layer, Vector3 _worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.up);
        return (_hit && _hit.transform.gameObject.layer == (int) _layer);
    }

    private Vector3 WorldPosCentredOnTile(Vector3 _worldPos)
    {
        Vector3Int _cellPos = _tilemap.WorldToCell(_worldPos);
        Vector3 _baseTilemapOffset = new Vector3(.5f, .5f, 0);

        return _tilemap.CellToWorld(_cellPos) + _baseTilemapOffset;
    }

    // CONNECTIONS

    public void RefreshConveyorConnectionsAroundWorldPos(Vector3 _worldPos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 _offset = new Vector3(i, j, 0);
                Conveyor _conv = GetConveyorAtWorldPos(_worldPos + _offset);

                if (_conv)
                {
                    _conv.RefreshConveyorConnections();
                }
            }
        }
    }
}
