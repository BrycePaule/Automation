using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// DEPRECIATED - DO NOT USE
// Use ConveyorReceivable component on the object instead

public interface IConveyorReceivable
{
    public bool CanReceiveItem();
    public void PlaceItem(Item _item);
}
