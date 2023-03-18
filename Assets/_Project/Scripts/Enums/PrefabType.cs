using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public enum PrefabType
    {
        NOT_SELECTED = 0,

        Conveyor = 100,
        ConveyorSlot = 101,
        Sink = 102,
        Spawner = 103,
        Drill = 104,

        Item = 200,
        GemPink = 201,
        GemBlack = 202,

        Marker = 900,
        ConnectionMarker = 901,
    }
}