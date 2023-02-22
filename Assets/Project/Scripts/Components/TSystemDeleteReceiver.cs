using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemDeleteReceiver : MonoBehaviour, ITSystemReceivable
{
    public void PlaceItem(Item _item)
    {
        Destroy(_item.gameObject);
    }

    public bool CanReceiveItem(Item _item)
    {
        if (!ItemPassesFilter(_item)) { return false; }

        return true;
    }

    public bool ItemPassesFilter(Item _item)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();

        if (_filter == null) { return true; } // no filter component
        if (_filter.ItemType == ItemType.Any) { return true; } // filter accepts anything
        if (_filter.ItemType == _item.ItemType) { return true; } // filter set to item

        return false;
    }
}
