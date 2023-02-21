using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemQueueReceiver : MonoBehaviour, ITSystemReceivable
{
    public int SlotCount;
    public Queue<ConveyorSlot> Slots;

    public void PlaceItem(Item _item)
    {
        // shuffle all but last slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            Slots.Enqueue(Slots.Dequeue());
        }

        // add item to last slot
        ConveyorSlot _lastSlot = Slots.Dequeue();

        _lastSlot.SetItem(_item);
        Slots.Enqueue(_lastSlot);
    }

    public bool CanReceiveItem()
    {
        // shuffle all but back slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            Slots.Enqueue(Slots.Dequeue());
        }

        // add item to back slot
        ConveyorSlot _lastSlot = Slots.Dequeue();

        bool _receivable = false;
        if (_lastSlot.IsEmpty())
        {
            _receivable = true;
        }

        Slots.Enqueue(_lastSlot);

        return _receivable;
    }
}
