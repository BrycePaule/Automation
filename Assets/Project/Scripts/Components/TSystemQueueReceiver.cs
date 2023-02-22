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

    public bool CanReceiveItem(Item _item)
    {
        if (!ItemPassesFilter(_item)) { return false; }

        // shuffle all but last slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            Slots.Enqueue(Slots.Dequeue());
        }

        // add item to last slot
        ConveyorSlot _lastSlot = Slots.Dequeue();

        bool _receivable = false;
        if (_lastSlot.IsEmpty())
        {
            _receivable = true;
        }

        Slots.Enqueue(_lastSlot);

        return _receivable;
    }

    public bool ItemPassesFilter(Item _item)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();

        if (_filter == null) { return true; } // no filter component
        if (_filter.ItemType == ItemType.Any) { return true; } // filter accepts anything
        if (_filter.ItemType == _item.ItemType) { return true; } // filter set to item

        return false;
    }
}
