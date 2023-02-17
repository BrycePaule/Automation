using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECIATED - DO NOT USE
// Use ConveyorConnectable component on the object instead

public interface IConveyorConnectable
{

    // Ensure all classess that implement this have a 'NextConveyor' variable


    public void RefreshPushConnection();

}
