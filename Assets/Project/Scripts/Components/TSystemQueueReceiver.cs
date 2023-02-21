using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemConnector))]
public class TSystemQueueReceiver : MonoBehaviour, ITSystemReceivable
{
    public int QueueSize;
    public Queue<ConveyorSlot> QueueSlots;

    private TSystemConnector tSysConnector;

    private void Awake()
    {
        tSysConnector = GetComponent<TSystemConnector>();
    }

    public void PlaceItem(Item _item)
    {
        // shuffle all but last slot
        for (int i = 0; i < QueueSize - 1; i++)
        {
            QueueSlots.Enqueue(QueueSlots.Dequeue());
        }

        // add item to last slot
        ConveyorSlot lastSlot = QueueSlots.Dequeue();
        lastSlot.SetItem(_item);

        tSysConnector.RefreshPushConnection();
        QueueSlots.Enqueue(lastSlot);
    }

    public bool CanReceiveItem()
    {
        bool _receivable = false;

        // shuffle all but back slot
        for (int i = 0; i < QueueSize - 1; i++)
        {
            QueueSlots.Enqueue(QueueSlots.Dequeue());
        }

        // add item to back slot
        ConveyorSlot _backSlot = QueueSlots.Dequeue();
        if (_backSlot.IsEmpty())
        {
            _receivable = true;
        }
        QueueSlots.Enqueue(_backSlot);

        return _receivable;
    }
}
