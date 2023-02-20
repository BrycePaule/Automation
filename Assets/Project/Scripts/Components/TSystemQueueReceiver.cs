using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(TSystemConnector))]
public class TSystemQueueReceiver : MonoBehaviour, ITSystemReceivable
{
    public int InventorySize;
    public Queue<ConveyorSlot> Inventory;

    private TSystemConnector convConnectable;

    private void Awake()
    {
        convConnectable.GetComponent<TSystemConnector>();
    }

    public void PlaceItem(Item _item)
    {
        // shuffle all but last slot
        for (int i = 0; i < InventorySize - 1; i++)
        {
            Inventory.Enqueue(Inventory.Dequeue());
        }

        // add item to last slot
        ConveyorSlot lastSlot = Inventory.Dequeue();
        lastSlot.SetItem(_item);

        GetComponent<TSystemConnector>().RefreshPushConnection();
        Inventory.Enqueue(lastSlot);
    }

    public bool CanReceiveItem()
    {
        bool _receivable = false;

        // shuffle all but back slot
        for (int i = 0; i < InventorySize - 1; i++)
        {
            Inventory.Enqueue(Inventory.Dequeue());
        }

        // add item to back slot
        ConveyorSlot _backSlot = Inventory.Dequeue();
        if (_backSlot.IsEmpty())
        {
            _receivable = true;
        }
        Inventory.Enqueue(_backSlot);

        return _receivable;
    }
}
