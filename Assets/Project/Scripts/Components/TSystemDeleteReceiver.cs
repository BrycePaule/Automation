using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemDeleteReceiver : MonoBehaviour, ITSystemReceivable
{
    [SerializeField] private GameEvent_ResourceType onResourceCollect;

    public void Give(Resource _item)
    {
        Destroy(_item.gameObject);
        onResourceCollect.Raise(_item.ItemType);
    }

    public bool CanReceive(ResourceType _itemType)
    {
        if (!ItemMatchesFilter(_itemType)) { return false; }

        return true;
    }

    public bool ItemMatchesFilter(ResourceType _itemType)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
        if (_filter == null) { return true; }
    
        return _filter.Check(_itemType);
    }
}
