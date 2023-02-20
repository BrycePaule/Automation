using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemReceivable
{
    public bool CanReceiveItem();
    public void PlaceItem(Item _item);
}
