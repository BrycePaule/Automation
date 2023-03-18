using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace bpdev
{
    [System.Serializable]
    [CreateAssetMenu(menuName="Scriptables/Map/MyTile", fileName="New MyTile")]
    public class TerrainTile : Tile
    {
        [Header("Custom Settings")]
        public bool Passable;
        public bool Buildable;
        
        [Header("Drillable settings")]
        public bool Drillable;
        public float Hardness;
        public ResourceType ResourceTypeReleased;

        public override void RefreshTile(Vector3Int position, ITilemap tilemap)
        {
            base.RefreshTile(position, tilemap);
        }

        public override void GetTileData(Vector3Int position, ITilemap tilemap, ref TileData tileData)
        {
            tileData.sprite = sprite;
        }
    }
}