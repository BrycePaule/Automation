using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap _tilemap;

    [SerializeField] private GameObject conveyerPrefab;


    // BUILD & REMOVE

    public Conveyor CreateConveyorAt(Vector3 _worldPos)
    {
        GameObject _convObj = Instantiate(conveyerPrefab, WorldPosAtTileCentre(_worldPos), Quaternion.identity);
        Conveyor _conv = _convObj.GetComponent<Conveyor>();
        _conv.SetReferences(this);

        RefreshConveyorsAroundCell(_tilemap.WorldToCell(_worldPos));

        return _conv;
    }

    public void DestroyConveyorAt(Vector3 _worldPos)
    {
        Conveyor _conv = GetConveyorAtWorldPos(_worldPos);

        if (_conv)
        {
            Destroy(_conv.gameObject);
        }
    }

    // CONVEYOR GETTERS

    public Conveyor GetConveyorAtScreenPos(Vector2 _screenPos)
    {
        Vector3 _worldPos = Camera.main.ScreenToWorldPoint(_screenPos);
        return GetConveyorAtWorldPos(_worldPos);
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
    
    // CONVEYOR CHECKERS

    public bool IsLayerAtWorldPos(Layers _layer, Vector3 _worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.up);
        return (_hit && _hit.transform.gameObject.layer == (int) _layer);
    }

    private void RefreshConveyorsAroundCell(Vector3Int _cellPos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 _offset = new Vector3(i, j, 0);
                Conveyor _adjacentConveyor = GetConveyorAtWorldPos(_cellPos + _offset);

                if (_adjacentConveyor)
                {
                    _adjacentConveyor.RefreshConveyorConnections();
                }
            }
        }
    }

    // HELPERS

    private Vector3 WorldPosAtTileCentre(Vector3 _worldPos)
    {
        Vector3Int _cellPos = _tilemap.WorldToCell(_worldPos);
        Vector3 _baseTilemapOffset = new Vector3(.5f, .5f, 0);

        return _tilemap.CellToWorld(_cellPos) + _baseTilemapOffset;
    }

}
