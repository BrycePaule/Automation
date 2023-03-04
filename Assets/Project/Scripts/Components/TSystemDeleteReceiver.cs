using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemDeleteReceiver : MonoBehaviour, ITSystemReceivable
{
    [SerializeField] private GameEvent_ResourceType onResourceCollect;

    public void Give(Resource resource)
    {
        Destroy(resource.gameObject);
        onResourceCollect.Raise(resource.resourceType);
    }

    public bool CanReceive(ResourceType resourceType)
    {
        if (!ItemMatchesFilter(resourceType)) { return false; }

        return true;
    }

    public bool ItemMatchesFilter(ResourceType resourceType)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
        if (_filter == null) { return true; }
    
        return _filter.Check(resourceType);
    }
}
