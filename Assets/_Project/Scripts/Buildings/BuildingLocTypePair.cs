using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    public struct BuildingLocTypePair
    {
        public Vector3Int Location;
        public BuildingType BuildingType;

        public BuildingLocTypePair(Vector3Int _location, BuildingType _type)
        {
            Location = _location;
            BuildingType = _type;
        }
    }
}
