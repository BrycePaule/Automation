using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemConnector))]
public class TSystemRotator : MonoBehaviour, ITSystemRotatable
{
    private TSystemConnector connector;

    private void Awake()
    {
        connector = GetComponent<TSystemConnector>();
    }

    public void RotateClockwise(bool _reverse = false)
    {
        int _currDir = (int) connector.Facing;
        int _newDir = (_currDir + 1) % 4;
        connector.Facing = (CardinalDirection) _newDir;

        if (_reverse)
        {
            transform.Rotate(new Vector3(0, 0, 90));
        }
        else
        {
            transform.Rotate(new Vector3(0, 0, -90));
        }

        connector.RefreshTSysConnection();
    }

    public void RotateAntiClockwise()
    {
        RotateClockwise(_reverse: true);
    }
}
