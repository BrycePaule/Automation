using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    [System.Serializable]
    public class Threshold
    {
        [Range(0f, 1f)] public float Value;	
    }
}