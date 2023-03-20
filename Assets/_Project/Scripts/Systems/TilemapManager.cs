using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace bpdev
{
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

        public Dictionary<Vector3Int, MapToken> TokenCache = new Dictionary<Vector3Int, MapToken>();
        public Dictionary<Vector3Int, GameObject> BuildingCache = new Dictionary<Vector3Int, GameObject>();

        protected override void Awake()
        {
            base.Awake();
        }

        private void OnEnable()
        {
            tilemap = FindObjectOfType<Tilemap>();
            RefreshTilesAroundPlayer(InputManager.Instance.PlayerCellPos);
        }

        private void Update()
        {
            if (InputManager.Instance.PlayerCellPos == null) { return; }
            if (InputManager.Instance.PlayerCellPosLastFrame == null) { return; }

            if (InputManager.Instance.PlayerCellPos != InputManager.Instance.PlayerCellPosLastFrame)
            {
                RefreshTilesAroundPlayer(InputManager.Instance.PlayerCellPos);
            }
        }

        // GENERATION

        public void SetTile(Vector2Int pos, MapToken token)
        {
            if (token == MapToken.Ground) { tilemap.SetTile((Vector3Int) pos, BaseTile); }
            if (token == MapToken.AlternateGround) { tilemap.SetTile((Vector3Int) pos, AltBaseTile); }
            if (token == MapToken.Gem1) { tilemap.SetTile((Vector3Int) pos, Gem1Tile); }
            if (token == MapToken.Gem2) { tilemap.SetTile((Vector3Int) pos, Gem2Tile); }
        }

        // This could probably be refactored to only update in the direction the player is moving
        public void RefreshTilesAroundPlayer(Vector3Int playerCellPos)
        {
            foreach (var pos in Utils.EvaluateGrid(playerCellPos.x - (mapSize / 2), playerCellPos.y - (mapSize / 2), mapSize))
            {
                if (!TokenCache.ContainsKey(pos))
                {
                    TokenCache[pos] = MapGenerator.Instance.GetTokenAtPos(pos, playerCellPos);
                }

                SetTile((Vector2Int) pos, TokenCache[pos]);
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
            BuildingCache[cellPos] = building;
        }

        public void DestroyBuilding(Vector3Int cellPos)
        {
            if (BuildingCache.ContainsKey(cellPos))
            {
                Destroy(BuildingCache[cellPos]);
                BuildingCache.Remove(cellPos);
            }
        }
        
        public GameObject GetBuilding(Vector3Int cellPos)
        {
            if (BuildingCache.ContainsKey(cellPos))
            {
                return BuildingCache[cellPos];
            }

            return null;
        }

        // CHECKERS

        public bool CanBuildAt(Vector3Int cellPos)
        {
            TerrainTile _tile = (TerrainTile) tilemap.GetTile(cellPos);

            if (!_tile.Buildable) { return false; }
            if (BuildingCache.ContainsKey(cellPos)) { return false; }

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
            return BuildingCache.ContainsKey(cellPos);
        }

    }
}