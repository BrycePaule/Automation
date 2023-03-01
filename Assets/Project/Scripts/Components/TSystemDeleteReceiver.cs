using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemDeleteReceiver : MonoBehaviour, ITSystemReceivable
{
    public void Give(Item _item)
    {
        Destroy(_item.gameObject);
    }

    public bool CanReceive(ItemType _itemType)
    {
        if (!ItemMatchesFilter(_itemType)) { return false; }

        return true;
    }

    public bool ItemMatchesFilter(ItemType _itemType)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
        if (_filter == null) { return true; }
    
        return _filter.Check(_itemType);
    }
}
