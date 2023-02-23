using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemDeleteReceiver : MonoBehaviour, ITSystemReceivable
{
    public void Give(Item _item)
    {
        Destroy(_item.gameObject);
    }

    public bool CanReceive(Item _item)
    {
        if (!ItemMatchesFilter(_item)) { return false; }

        return true;
    }

    public bool ItemMatchesFilter(Item _item)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
        if (_filter == null) { return true; }
    
        return _filter.Check(_item);
    }
}
