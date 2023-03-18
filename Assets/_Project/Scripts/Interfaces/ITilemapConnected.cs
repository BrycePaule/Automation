using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace bpdev
{
    public interface ITilemapConnected
    {
        public void SetTilemap(Tilemap tilemap);
    }
}