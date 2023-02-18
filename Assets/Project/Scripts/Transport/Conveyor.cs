using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

[RequireComponent(typeof(Rotatable))]
[RequireComponent(typeof(ConveyorConnectable))]
[RequireComponent(typeof(ConveyorReceivable))]
public class Conveyor : MonoBehaviour
{
    /*
        For a 5 block Conveyor:

        x o o x x
        4 3 2 1 0

        Travelling in >>>> direction
    */

    public int SlotCount;
    public float ItemsMovedPerSecond;

    private Vector3Int cellPos;
    private Queue<ConveyorSlot> Slots;

    [Header("References")]
    [SerializeField] private ConveyorManager convManager;
    [SerializeField] private GameObject slotPrefab;

    private ConveyorConnectable convConnectable;
    int stackedItems = 0;

    [Header("Debug")]
    [SerializeField] private bool showSlotGraphics;

    private void Awake()
    {
        convConnectable = GetComponent<ConveyorConnectable>();
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
        Vector3 _moveOffset = new Vector3((ItemsMovedPerSecond / Slots.Count) * Time.deltaTime, 0f, 0f);
        Vector3 _resetPos = new Vector3(-0.5f, 0f, 0f);
        float _xResetThreshold = 1f;

        // CALCULATE STACKED ITEMS
        stackedItems = 0;
        if (!CanOffloadItem())
        {
            stackedItems = CountItemsStackedAtFront();
        }

        _xResetThreshold = 0.5f - ((1f / SlotCount) * stackedItems);

        List<ConveyorSlot> _slotCache = new List<ConveyorSlot>();
        ConveyorSlot _resetSlot = null;

        // UPDATE SLOTS AND MOVE ITEMS
        for (int i = 0; i <= SlotCount - 1; i++)
        {
            ConveyorSlot _slot = Slots.Dequeue();

            if (i < stackedItems)
            {
                _slotCache.Add(_slot);
                continue;
            }

            if (_slot.transform.localPosition.x >= _xResetThreshold)
            {
                if (convConnectable.NextConveyor != null)
                {
                    convConnectable.NextConveyor.PlaceItem(_slot.GetItem());
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
            if (_slot != null)
            {
                Slots.Enqueue(_slot);
            }
        }

        if (_resetSlot != null)
        {
            Slots.Enqueue(_resetSlot);
        }

    }

    private int CountItemsStackedAtFront()
    {
        int _stackedItems = 0;

        for (int i = 0; i <= SlotCount - 1; i++)
        {
            ConveyorSlot _slot = Slots.Dequeue();

            if (_slot.IsNotEmpty() && _stackedItems == i)
            {
                _stackedItems += 1;
            }

            Slots.Enqueue(_slot);
        }

        return _stackedItems;
    }

    public void PlaceItem(Item _item)
    {
        // shuffle all but last slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            Slots.Enqueue(Slots.Dequeue());
        }

        // add item to last slot
        ConveyorSlot lastSlot = Slots.Dequeue();
        lastSlot.SetItem(_item);

        GetComponent<ConveyorConnectable>().RefreshPushConnection();
        Slots.Enqueue(lastSlot);
    }

    // HELPERS

    public bool CanReceiveItem()
    {
        bool _receivable = false;

        // shuffle all but back slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            Slots.Enqueue(Slots.Dequeue());
        }

        // add item to back slot
        ConveyorSlot _backSlot = Slots.Dequeue();
        if (_backSlot.IsEmpty())
        {
            _receivable = true;
        }
        Slots.Enqueue(_backSlot);

        return _receivable;
    }

    private void CreateAndPlaceSlots()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            Vector3 _positionOffset = new Vector3(-0.5f + ((1f / SlotCount) * (i + 1f)), 0f, 0f);
            GameObject slotObj = Instantiate(slotPrefab, transform.position + _positionOffset, Quaternion.identity);
            slotObj.transform.parent = transform;
            slotObj.name = slotPrefab.name + "_" + i;
            slotObj.transform.localScale = new Vector3(
                slotObj.transform.localScale.x / SlotCount,
                slotObj.transform.localScale.y,
                slotObj.transform.localScale.z);
        }
    }

    private void EstablishSlotQueue()
    {
        Slots = new Queue<ConveyorSlot>(SlotCount);

        ConveyorSlot[] _slots = GetComponentsInChildren<ConveyorSlot>();
        for (int i = SlotCount - 1; i >= 0; i--)
        {
            Slots.Enqueue(_slots[i])   ;
        }
    }

    private void ToggleSlotSprites()
    {
        if (showSlotGraphics)
        {
            foreach (ConveyorSlot slot in Slots)
            {
                slot.ShowSprite();
            }
        }
        else
        {
            foreach (ConveyorSlot slot in Slots)
            {
                slot.HideSprite();
            }
        }
    }

    private bool CanOffloadItem()
    {
        Conveyor _nextConv = convConnectable.NextConveyor;
        if (_nextConv == null) { return false; }
        if (!_nextConv.CanReceiveItem()) { return false; }

        return true;
    }

}
