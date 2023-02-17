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

        5 4 3 2 1

        Travelling in >>>> direction
    */

    public int SlotCount;
    public float ItemsMovedPerSecond;

    private Queue<ConveyorSlot> Slots;

    private Vector3Int cellPos;

    [Header("References")]
    [SerializeField] private ConveyorManager conveyorManager;
    [SerializeField] private GameObject slotPrefab;

    private ConveyorConnectable conveyorConnectable;

    [Header("Debug")]
    [SerializeField] private bool showSlotGraphics;

    private void Awake()
    {
        conveyorConnectable = GetComponent<ConveyorConnectable>();
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
        // something problematic where if it fails to move item to next belt
        // then it just permanently resets to the back and the item is stuck on the belt


        // move all slots along
        for (int i = 0; i < SlotCount; i++)
        {
            ConveyorSlot slot = Slots.Dequeue();
            slot.transform.localPosition += new Vector3((ItemsMovedPerSecond / Slots.Count) * Time.deltaTime, 0f, 0f);
            Slots.Enqueue(slot);
        }

        ConveyorSlot _frontSlot = Slots.Peek();
        if (_frontSlot.transform.localPosition.x <= .5f) { return; }

        if (_frontSlot.IsNotEmpty())
        {
            if (conveyorConnectable.NextConveyor != null) // item at front moving to next belt
            {
                conveyorConnectable.NextConveyor.PlaceItem(_frontSlot.GetItem());
                _frontSlot.Clear();
            }
            else // item stuck at front with no next belt
            {
                ShuffleAllItemsBackOne();
            }
        }

        _frontSlot.transform.localPosition -= new Vector3(1f, 0f, 0f);
        Slots.Dequeue();
        Slots.Enqueue(_frontSlot);
    }

    public void PlaceItem(Item _item)
    {
        // shuffle all but last slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            ConveyorSlot slotToShuffle = Slots.Dequeue();
            Slots.Enqueue(slotToShuffle);
        }

        // add item to last slot
        ConveyorSlot lastSlot = Slots.Dequeue();
        lastSlot.SetItem(_item);

        GetComponent<ConveyorConnectable>().RefreshPushConnection();
        Slots.Enqueue(lastSlot);
    }


    private void ShuffleAllItemsBackOne()
    {
        ConveyorSlot slotCurr;
        ConveyorSlot slotNext;
        Item itemCurr;
        Item itemNext;


        slotCurr = Slots.Dequeue();
        itemCurr = slotCurr.GetItem();
        slotCurr.Clear();

        for (int i = SlotCount - 2; i >= 0; i--)
        {
            slotNext = Slots.Dequeue();
            itemNext = slotNext.GetItem();
            slotNext.Clear();

            slotNext.SetItem(itemCurr);
            Slots.Enqueue(slotCurr);

            itemCurr = itemNext;
            slotCurr = slotNext;
        }

        Slots.Enqueue(slotCurr);
    }


    // HELPERS

    public void SetReferences(ConveyorManager _convManager, Vector3Int _cellPos)
    {
        conveyorManager = _convManager;
        cellPos = _cellPos;
    }

    public bool CanReceiveItem()
    {
        bool _receivable = false;

        // shuffle all but last slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            ConveyorSlot slotToShuffle = Slots.Dequeue();
            Slots.Enqueue(slotToShuffle);
        }

        // add item to last slot
        ConveyorSlot lastSlot = Slots.Dequeue();
        if (lastSlot.IsEmpty())
        {
            _receivable = true;
        }
        Slots.Enqueue(lastSlot);

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

}
