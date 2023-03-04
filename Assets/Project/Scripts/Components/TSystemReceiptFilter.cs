using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(ITSystemReceivable))]
public class TSystemReceiptFilter : MonoBehaviour
{
    public ResourceType ResourceType;

    public bool Check(ResourceType resourceType)
    {
        if (ResourceType == ResourceType.Any) { return true; }
        if (ResourceType == resourceType) { return true; }

        return false;
    }
}
