using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemReceivable
{
    public void PlaceItem(Item _item);

    public bool CanReceiveItem(Item _item);
    public bool ItemPassesFilter(Item _item);
}
