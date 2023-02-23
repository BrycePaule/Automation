using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITSystemReceivable))]
public class TSystemReceiptFilter : MonoBehaviour
{
    public ItemType ItemType;

    public bool Check(Item _item)
    {
        if (ItemType == ItemType.Any) { return true; }
        if (ItemType == _item.ItemType) { return true; }

        return false;
    }
}
