using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TSystemManager : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Tilemap tilemap;
    [SerializeField] private TilemapManager tilemapManager;
    [SerializeField] private GameObject markerPrefab;


    // CREATION, REMOVAL, INTERACTION

    public void CreateConnectableAtWorldPos(GameObject _prefab, Vector3 _worldPos)
    {
        Vector3Int _cellPos = tilemap.WorldToCell(_worldPos);

        GameObject _connectable = Instantiate(_prefab, tilemapManager.TileAnchorFromWorldPos(_worldPos), Quaternion.identity);
        _connectable.name = _prefab.name + " " + _cellPos;

        RefreshConnectionsAroundWorldPos(_worldPos);

    }

    public void DestroyConnectableAtWorldPos(Vector3 _worldPos)
    {
        TSystemConnector _connectable = GetConnectableAtWorldPos(_worldPos);

        if (_connectable)
        {
            Destroy(_connectable.gameObject);
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

    public TSystemConnector GetConnectableAtWorldPos(Vector3 _worldPos)
    {
        TSystemConnector _connectable = null;

        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.up);
        if (_hit)
        {
            _hit.transform.TryGetComponent<TSystemConnector>(out _connectable);
        }
        
        return _connectable;
    }


    // CONNECTIONS

    public void RefreshConnectionsAroundWorldPos(Vector3 _worldPos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3 _offset = new Vector3(i, j, 0);
                TSystemConnector _conv = GetConnectableAtWorldPos(_worldPos + _offset);

                if (_conv)
                {
                    _conv.GetComponent<TSystemConnector>().RefreshPushConnection();
                }
            }
        }
    }
}
