using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class TSystemManager : Singleton<TSystemManager>
{
    [SerializeField] private GameObject markerPrefab;

    public void RefreshConnectionsAround(Vector3Int cellPos)
    {
        for (int i = -1; i <= 1; i++)
        {
            for (int j = -1; j <= 1; j++)
            {
                Vector3Int _offset = new Vector3Int(j, i, 0);
                Vector3Int _cellToCheck = cellPos + _offset;

                GameObject m = Instantiate(markerPrefab, TilemapManager.Instance.TileAnchorFromCellPos(cellPos) + _offset, Quaternion.identity);
                Destroy(m, 2f);

                if (!TilemapManager.Instance.InsideBounds(_cellToCheck)) { continue; }

                GameObject _building = BuildingProxy.Instance.GetBuildingAt(_cellToCheck);

                if (_building != null)
                {
                    _building.GetComponent<ITSystemConnectable>().RefreshTSysConnection();
                }
            }
        }
    }
}
