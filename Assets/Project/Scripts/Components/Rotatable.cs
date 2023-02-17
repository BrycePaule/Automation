using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ConveyorConnectable))]
public class Rotatable : MonoBehaviour
{

    private ConveyorConnectable conveyorConnectable;

    private void Awake()
    {
        conveyorConnectable = GetComponent<ConveyorConnectable>();
    }

    public void RotateClockwise()
    {
        int currDir = (int) conveyorConnectable.Facing;
        int newDir = (currDir + 1) % 4;

        conveyorConnectable.Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, -90));

        conveyorConnectable.RefreshPushConnection();
    }

    public void RotateAntiClockwise()
    {
        int currDir = (int) conveyorConnectable.Facing;
        int newDir = (currDir + 3) % 4;

        conveyorConnectable.Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, 90));

        conveyorConnectable.RefreshPushConnection();
    }

}
