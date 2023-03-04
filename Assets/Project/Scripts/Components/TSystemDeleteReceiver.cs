using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemDeleteReceiver : MonoBehaviour, ITSystemReceivable
{
    [SerializeField] private GameEvent_ItemType onResourceCollect;

    public void Give(Item _item)
    {
        Destroy(_item.gameObject);
        onResourceCollect.Raise(_item.ItemType);
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
