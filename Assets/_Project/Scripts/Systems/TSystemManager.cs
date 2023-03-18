using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class TSystemManager : Singleton<TSystemManager>
    {
        [Header("Debug")]
        [SerializeField] private bool markRefreshCells;

        [Header("References")]
        [SerializeField] private GameObject markerPrefab;

        public void RefreshConnectionsAround(Vector3Int cellPos)
        {
            for (int i = -1; i <= 1; i++)
            {
                for (int j = -1; j <= 1; j++)
                {
                    Vector3Int _offset = new Vector3Int(j, i, 0);
                    Vector3Int _cellToCheck = cellPos + _offset;

                    if (markRefreshCells)
                    {
                        GameObject m = Instantiate(markerPrefab, TilemapManager.Instance.TileAnchorFromCellPos(cellPos) + _offset, Quaternion.identity);
                        Destroy(m, 1f);
                    }

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
}