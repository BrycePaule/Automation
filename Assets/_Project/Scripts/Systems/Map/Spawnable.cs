using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    public class Spawnable
    {
        public Object Object;
        public float SpawnChance;
        // public List<TileType> SpawnableOn;
        public bool Exclusive;

        public bool RandomiseSprites;
        public List<Sprite> Sprites;
    }
}