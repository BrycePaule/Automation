using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemReceivable
{
    public void Give(Item _item);

    // CHECKERS
    public bool CanReceive(ItemType _itemType);
    public bool ItemMatchesFilter(ItemType _itemType);
}
