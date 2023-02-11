using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Item : MonoBehaviour
{
    public Vector3 StartPos;
    public Vector3 TargetPos;

    public bool IsAtTarget() => transform.position == TargetPos;
}
