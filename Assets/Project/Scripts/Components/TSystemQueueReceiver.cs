using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemQueueReceiver : MonoBehaviour, ITSystemReceivable
{
    public Queue<ConveyorSlot> Slots;
    public int SlotCount;

    public void Give(Resource _item)
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

    public bool CanReceive(ResourceType _itemType)
    {
        if (!ItemMatchesFilter(_itemType)) { return false; }

        ConveyorSlot _back = null;
        for (int i = 1; i <= SlotCount; i++)
        {
            ConveyorSlot _curr = Slots.Dequeue();
            Slots.Enqueue(_curr);

            if (i == SlotCount)
            {
                _back = _curr;
            }
        }

        return _back.IsEmpty();

        // float _leftPickupBound = -0.5f;
        // float _rightPickupBound = -0.5f + (1 / SlotCount);

        // float _backXPos = _back.transform.position.x;

        // if (_backXPos >= _leftPickupBound && _backXPos <= _rightPickupBound)
        // {
        //     return _back.IsEmpty();
        // }

        // return false;
    }

    public bool ItemMatchesFilter(ResourceType _itemType)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
        if (_filter == null) { return true; }
    
        return _filter.Check(_itemType);
    }
}
