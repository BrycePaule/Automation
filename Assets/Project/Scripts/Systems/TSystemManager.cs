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

    public void PlaceTSystemObjectAtWorldPos(GameObject _prefab, Vector3 _worldPos)
    {
        if (!tilemapManager.CanBuildAt(_worldPos)) { return; }

        Vector3Int _cellPos = tilemapManager.CellFromWorldPos(_worldPos);

        GameObject _connectable = Instantiate(_prefab, tilemapManager.TileAnchorFromWorldPos(_worldPos), Quaternion.identity);
        _connectable.name = _prefab.name + " " + _cellPos;

        RefreshConnectionsAroundWorldPos(_worldPos);
    }

    public void PlaceTSystemObjectAtWorldPos(GameObject _prefab, Vector3 _worldPos, Tilemap _tilemap)
    {
        if (!tilemapManager.CanBuildAt(_worldPos)) { return; }

        Vector3Int _cellPos = tilemapManager.CellFromWorldPos(_worldPos);

        GameObject _connectable = Instantiate(_prefab, tilemapManager.TileAnchorFromWorldPos(_worldPos), Quaternion.identity);
        _connectable.name = _prefab.name + " " + _cellPos;
        _connectable.GetComponent<ITilemapConnected>().SetTilemap(tilemap);
        
        RefreshConnectionsAroundWorldPos(_worldPos);
    }

    public void DestroyTSystemObjectAtWorldPos(Vector3 _worldPos)
    {
        Component _connectable = GetTSystemObjectAtWorldPos(_worldPos);

        if (_connectable != null)
        {
            Destroy(_connectable.gameObject);
            RefreshConnectionsAroundWorldPos(_worldPos);
        }
    }

    public Component GetTSystemObjectAtWorldPos(Vector3 _worldPos)
    {
        RaycastHit2D _hit = Physics2D.Raycast(_worldPos, Vector2.zero);

        if (!_hit) { return null; }

        if (_hit.transform.GetComponent<ITSystemConnectable>() != null)
        {
           return (Component) _hit.transform.GetComponent<ITSystemConnectable>();
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
                Component _connectable = GetTSystemObjectAtWorldPos(_worldPos + _offset);

                if (_connectable != null)
                {
                    _connectable.GetComponent<TSystemConnector>().RefreshTSysConnection();
                }
            }
        }
    }
}
