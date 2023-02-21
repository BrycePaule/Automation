using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemConnector))]
public class TSystemRotator : MonoBehaviour, ITSystemRotatable
{
    private TSystemConnector conveyorConnectable;

    private void Awake()
    {
        conveyorConnectable = GetComponent<TSystemConnector>();
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
