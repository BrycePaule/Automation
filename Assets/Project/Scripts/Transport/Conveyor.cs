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

        back    |   x o o x x   |   front (first slot if you dequeue)
        back    |   4 3 2 1 0   |   front (first slot if you dequeue)

        Travelling in >>>> direction
    */

    // potentially to think of a way that this movespeed doesn't scale with the amount of slots
    // currently if you bump up the amount of slots, items take significantly longer 
    // to move across a single conveyor
    public float ItemsMovedPerSecond;

    private int stackedItems = 0;

    private TSystemConnector connector;
    private TSystemQueueReceiver receiver;

    [Header("Debug")]
    [SerializeField] private bool showSlotGraphics;

    private void Awake()
    {
        connector = GetComponent<TSystemConnector>();
        receiver = GetComponent<TSystemQueueReceiver>();
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
        Vector3 _moveOffset = new Vector3((ItemsMovedPerSecond / receiver.SlotCount) * Time.deltaTime, 0f, 0f);
        Vector3 _resetPos = new Vector3(-0.5f, 0f, 0f);

        stackedItems = CountStackedItems();
        float _xResetThreshold = 0.5f - ((1f / receiver.SlotCount) * stackedItems);

        List<ConveyorSlot> _slotCache = new List<ConveyorSlot>();
        ConveyorSlot _resetSlot = null;
        ConveyorSlot _front = receiver.Slots.Peek();

        for (int i = 0; i <= receiver.SlotCount - 1; i++)
        {
            ConveyorSlot _slot = receiver.Slots.Dequeue();

            // if slot is part of the front stack
            if (i < stackedItems)
            {
                if (_slot == _front && connector.CanOffloadItem(_slot.Item))
                {
                    connector.GetConnectedReceiver().Give(_slot.Item);
                    _slot.ClearItem();
                }

                _slotCache.Add(_slot);
                continue;
            }

            // handling the rest
            if (_slot.transform.localPosition.x >= _xResetThreshold)
            {
                _slot.transform.localPosition = _resetPos;
                _slot.transform.localPosition += _moveOffset;
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
            receiver.Slots.Enqueue(_slot);
        }

        if (_resetSlot != null)
        {
            receiver.Slots.Enqueue(_resetSlot);
        }
    }

    private int CountStackedItems()
    {
        int _stackedItems = 0;

        for (int i = 0; i <= receiver.SlotCount - 1; i++)
        {
            ConveyorSlot _slot = receiver.Slots.Dequeue();

            if (_slot.IsNotEmpty() && _stackedItems == i)
            {
                _stackedItems += 1;
            }

            receiver.Slots.Enqueue(_slot);
        }

        return _stackedItems;
    }

    private void CreateAndPlaceSlots()
    {
        GameObject _slotPrefab = GetComponentInChildren<ConveyorSlot>().gameObject;
        _slotPrefab.transform.position = transform.position + new Vector3(-0.5f + ((1f / receiver.SlotCount) * (receiver.SlotCount + 1f)), 0f, 0f);

        for (int i = 0; i < receiver.SlotCount - 1; i++)
        {
            Vector3 _positionOffset = new Vector3(-0.5f + ((1f / receiver.SlotCount) * (i + 1f)), 0f, 0f);
            GameObject slotObj = Instantiate(_slotPrefab, transform.position + _positionOffset, Quaternion.identity);
            slotObj.transform.parent = transform;
        }

        ConveyorSlot[] _slots = GetComponentsInChildren<ConveyorSlot>();

        for (int i = 0; i < receiver.SlotCount; i++)
        {
            _slots[i].transform.name = _slotPrefab.name + "_" + i;
            _slots[i].transform.localScale = new Vector3(
                _slots[i].transform.localScale.x / receiver.SlotCount,
                _slots[i].transform.localScale.y,
                _slots[i].transform.localScale.z);
        }
    }

    private void EstablishSlotQueue()
    {
        receiver.Slots = new Queue<ConveyorSlot>(receiver.SlotCount);

        ConveyorSlot[] _slots = GetComponentsInChildren<ConveyorSlot>();
        for (int i = receiver.SlotCount - 1; i >= 0; i--)
        {
            receiver.Slots.Enqueue(_slots[i])   ;
        }
    }

    private void ToggleSlotSprites()
    {
        if (showSlotGraphics)
        {
            foreach (ConveyorSlot slot in receiver.Slots)
            {
                slot.ShowSprite();
            }
        }
        else
        {
            foreach (ConveyorSlot slot in receiver.Slots)
            {
                slot.HideSprite();
            }
        }
    }


}
