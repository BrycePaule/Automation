using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[RequireComponent(typeof(TSystemRotator))]
[RequireComponent(typeof(TSystemConnector))]
[RequireComponent(typeof(TSystemQueueReceiver))]
public class Conveyor : MonoBehaviour
{
    /*
        For a 5 block Conveyor:

        x o o x x
        4 3 2 1 0

        Travelling in >>>> direction
    */

    public float ItemsMovedPerSecond;

    private Vector3Int cellPos;

    [Header("References")]
    [SerializeField] private GameObject slotPrefab;

    private TSystemConnector convConnectable;
    private TSystemQueueReceiver convReceivable;
    int stackedItems = 0;

    [Header("Debug")]
    [SerializeField] private bool showSlotGraphics;

    private void Awake()
    {
        convConnectable = GetComponent<TSystemConnector>();
        convReceivable = GetComponent<TSystemQueueReceiver>();
    }

    private void Start()
    {
        CreateAndPlaceSlots();
        EstablishSlotQueue();
    }

    private void FixedUpdate()
    {
        ToggleSlotSprites();
        MoveSlotsAlongConveyor();
    }

    // ITEM MOVEMENT

    private void MoveSlotsAlongConveyor()
    {
        Vector3 _moveOffset = new Vector3((ItemsMovedPerSecond / convReceivable.QueueSize) * Time.deltaTime, 0f, 0f);
        Vector3 _resetPos = new Vector3(-0.5f, 0f, 0f);
        float _xResetThreshold = 1f;

        // CALCULATE STACKED ITEMS
        stackedItems = 0;
        if (!CanOffloadItem())
        {
            stackedItems = CountItemsStackedAtFront();
        }

        _xResetThreshold = 0.5f - ((1f / convReceivable.QueueSize) * stackedItems);

        List<ConveyorSlot> _slotCache = new List<ConveyorSlot>();
        ConveyorSlot _resetSlot = null;

        // UPDATE SLOTS AND MOVE ITEMS
        for (int i = 0; i <= convReceivable.QueueSize - 1; i++)
        {
            ConveyorSlot _slot = convReceivable.QueueSlots.Dequeue();

            if (i < stackedItems)
            {
                _slotCache.Add(_slot);
                continue;
            }

            if (_slot.transform.localPosition.x >= _xResetThreshold)
            {
                if (convConnectable.NextConveyor != null)
                {
                    ((Component) convConnectable.NextConveyor).GetComponent<TSystemQueueReceiver>().PlaceItem(_slot.GetItem());
                    _slot.ClearItem();
                }

                _slot.transform.localPosition = _resetPos;
                _resetSlot = _slot;
            }
            else
            {
                _slot.transform.localPosition += _moveOffset;
                _slotCache.Add(_slot);
            }
        }

        // ENQUEUE SLOTS IS NEW ORDER
        foreach (ConveyorSlot _slot in _slotCache)
        {
            convReceivable.QueueSlots.Enqueue(_slot);
        }

        if (_resetSlot != null)
        {
            convReceivable.QueueSlots.Enqueue(_resetSlot);
        }

    }

    private int CountItemsStackedAtFront()
    {
        int _stackedItems = 0;

        for (int i = 0; i <= convReceivable.QueueSize - 1; i++)
        {
            ConveyorSlot _slot = convReceivable.QueueSlots.Dequeue();

            if (_slot.IsNotEmpty() && _stackedItems == i)
            {
                _stackedItems += 1;
            }

            convReceivable.QueueSlots.Enqueue(_slot);
        }

        return _stackedItems;
    }


    private void CreateAndPlaceSlots()
    {
        for (int i = 0; i < convReceivable.QueueSize; i++)
        {
            Vector3 _positionOffset = new Vector3(-0.5f + ((1f / convReceivable.QueueSize) * (i + 1f)), 0f, 0f);
            GameObject slotObj = Instantiate(slotPrefab, transform.position + _positionOffset, Quaternion.identity);
            slotObj.transform.parent = transform;
            slotObj.name = slotPrefab.name + "_" + i;
            slotObj.transform.localScale = new Vector3(
                slotObj.transform.localScale.x / convReceivable.QueueSize,
                slotObj.transform.localScale.y,
                slotObj.transform.localScale.z);
        }
    }

    private void EstablishSlotQueue()
    {
        convReceivable.QueueSlots = new Queue<ConveyorSlot>(convReceivable.QueueSize);

        ConveyorSlot[] _slots = GetComponentsInChildren<ConveyorSlot>();
        for (int i = convReceivable.QueueSize - 1; i >= 0; i--)
        {
            convReceivable.QueueSlots.Enqueue(_slots[i])   ;
        }
    }

    private void ToggleSlotSprites()
    {
        if (showSlotGraphics)
        {
            foreach (ConveyorSlot slot in convReceivable.QueueSlots)
            {
                slot.ShowSprite();
            }
        }
        else
        {
            foreach (ConveyorSlot slot in convReceivable.QueueSlots)
            {
                slot.HideSprite();
            }
        }
    }

    private bool CanOffloadItem()
    {
        ITSystemConnectable _nextConv = convConnectable.NextConveyor;
        if (_nextConv == null) { return false; }
        if (!((Component) _nextConv).GetComponent<TSystemQueueReceiver>().CanReceiveItem()) { return false; }

        return true;
    }

}
