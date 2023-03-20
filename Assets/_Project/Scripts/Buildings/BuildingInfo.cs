using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    public struct BuildingInfo
    {
        public BuildingType Type;
        public CardinalDirection Direction;
        public Vector3Int Location;

        public BuildingInfo(BuildingType _type, CardinalDirection _dir, Vector3Int _loc)
        {
            Type = _type;
            Direction = _dir;
            Location = _loc;
        }
    }
}
