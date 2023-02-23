using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface ITSystemReceivable
{
    public void Give(Item _item);

    // CHECKERS
    public bool CanReceive(Item _item);
    public bool ItemMatchesFilter(Item _item);
}
