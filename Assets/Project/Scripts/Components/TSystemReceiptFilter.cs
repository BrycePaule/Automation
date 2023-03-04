using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITSystemReceivable))]
public class TSystemReceiptFilter : MonoBehaviour
{
    public ResourceType ItemType;

    public bool Check(ResourceType _itemType)
    {
        if (ItemType == ResourceType.Any) { return true; }
        if (ItemType == _itemType) { return true; }

        return false;
    }
}
