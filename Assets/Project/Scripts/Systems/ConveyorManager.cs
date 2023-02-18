using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ConveyorManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap _tilemap;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private GameObject conveyerPrefab;
    [SerializeField] private GameObject markerPrefab;


    // CREATION, REMOVAL, INTERACTION

    public Conveyor CreateConveyorAtWorldPos(Vector3 _worldPos)
    {
        Vector3Int _cellPos = _tilemap.WorldToCell(_worldPos);

        GameObject _convObj = Instantiate(conveyerPrefab, tilemapManager.TileAnchorFromWorldPos(_worldPos), Quaternion.identity);
        Conveyor _conv = _convObj.GetComponent<Conveyor>();
        _conv.name = "Conveyor: " + _cellPos;

        RefreshConnectionsAroundWorldPos(_worldPos);

        return _conv;
    }

    public void CreateConnectableAtWorldPos(GameObject _prefab, Vector3 _worldPos)
    {
        Vector3Int _cellPos = _tilemap.WorldToCell(_worldPos);

        GameObject _connectable = Instantiate(_prefab, tilemapManager.TileAnchorFromWorldPos(_worldPos), Quaternion.identity);
        _connectable.name = _prefab.name + " " + _cellPos;

        RefreshConnectionsAroundWorldPos(_worldPos);

    }

    public void DestroyConveyorAtWorldPos(Vector3 _worldPos)
    {
        Conveyor _conv = GetConveyorAtWorldPos(_worldPos);

        if (_conv)
        {
            Destroy(_conv.gameObject);
            RefreshConnectionsAroundWorldPos(_worldPos);
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

    public ConveyorConnectable GetConnectableAtWorldPos(Vector3 _worldPos)
    {
        ConveyorConnectable _connectable;

        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.up);
        if (_hit)
        {
            _hit.transform.TryGetComponent<ConveyorConnectable>(out _connectable);
        }
        
        return null;
    }


    // CONNECTIONS

    public void RefreshConnectionsAroundWorldPos(Vector3 _worldPos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 _offset = new Vector3(i, j, 0);
                ConveyorConnectable _conv = GetConnectableAtWorldPos(_worldPos + _offset);

                if (_conv)
                {
                    _conv.GetComponent<ConveyorConnectable>().RefreshPushConnection();
                }
            }
        }
    }
}
