using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemReceivable
{
    public void Give(Resource resource);

    // CHECKERS
    public bool CanReceive(ResourceType resourceType);
    public bool ItemMatchesFilter(ResourceType resourceType);
}
