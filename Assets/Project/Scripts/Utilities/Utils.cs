using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class Utils 
{

    public static Vector3 DirToVector(CardinalDirection dir)
    {
        switch (dir)
        {
            case CardinalDirection.North:
                return new Vector3(0, 1, 0);
            case CardinalDirection.East:
                return new Vector3(1, 0, 0);
            case CardinalDirection.South:
                return new Vector3(0, -1, 0);
            case CardinalDirection.West:
                return new Vector3(-1, 0, 0);

            default:
                return new Vector3(0, 0, 0);
        }
    }

}
