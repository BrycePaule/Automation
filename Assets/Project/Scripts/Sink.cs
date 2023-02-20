using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemRotate))]
[RequireComponent(typeof(TSystemConnector))]
[RequireComponent(typeof(TSystemQueueReceiver))]
public class Sink : MonoBehaviour
{
    public void PlaceItem(Item _item)
    {
        Destroy(_item.gameObject);
    }
}
