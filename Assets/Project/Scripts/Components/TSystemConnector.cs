using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemRotator))]
public class TSystemConnector : MonoBehaviour, ITSystemConnectable
{
    public ITSystemConnectable NextConveyor;
    public CardinalDirection Facing = CardinalDirection.East;

    private TSystemRotator rotatable;

    private void Awake()
    {
        rotatable = GetComponent<TSystemRotator>();
    }

    public void RefreshPushConnection()
    {
        NextConveyor = null;

        if (rotatable)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Utils.DirToVector(Facing), Vector2.up);
            if (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer)
            {
                NextConveyor = hit.transform.GetComponent<ITSystemConnectable>();
            }
        }

    }
}
