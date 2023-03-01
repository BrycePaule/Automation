using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITSystemReceivable))]
public class TSystemReceiptFilter : MonoBehaviour
{
    public ItemType ItemType;

    public bool Check(ItemType _itemType)
    {
        if (ItemType == ItemType.Any) { return true; }
        if (ItemType == _itemType) { return true; }

        return false;
    }
}
