using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public interface ITSystemReceivable
    {
        public void Give(Resource resource);

        // CHECKERS
        public bool CanReceive(ResourceType resourceType);
        public bool ItemMatchesFilter(ResourceType resourceType);
    }
}