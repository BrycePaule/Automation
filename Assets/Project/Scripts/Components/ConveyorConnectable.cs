using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorConnectable : MonoBehaviour
{
    public Conveyor NextConveyor;
    public CardinalDirection Facing = CardinalDirection.East;

    private Rotatable rotatable;

    private void Awake()
    {
        rotatable = GetComponent<Rotatable>();
    }

    public void RefreshPushConnection()
    {
        NextConveyor = null;

        if (rotatable)
        {
            RaycastHit2D hit = Physics2D.Raycast(transform.position + Utils.DirToVector(Facing), Vector2.up);
            if (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer)
            {
                NextConveyor = hit.transform.GetComponent<Conveyor>();
            }
        }

    }
}
