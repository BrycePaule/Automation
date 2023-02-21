using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemNullReceiver : MonoBehaviour, ITSystemReceivable
{
    public bool CanReceiveItem() => true;

    public void PlaceItem(Item _item)
    {
        Destroy(_item.gameObject);
    }
}
