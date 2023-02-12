using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using UnityEditor;

public class Conveyor : MonoBehaviour
{
    public int SlotCount;
    public float ItemsMovedPerSecond;

    public Conveyor NextConveyor;
    public CardinalDirection Facing = CardinalDirection.East;
    public Queue<ConveyorSlot> SlotQueue;

    private Vector3Int cellPos;

    [Header("References")]
    [SerializeField] private ConveyorManager conveyorManager;
    [SerializeField] private GameObject slotPrefab;

    [Header("Debug")]
    [SerializeField] private bool showSlotGraphics;

    private void Awake()
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
        // move all slots along
        for (int i = 0; i < SlotCount; i++)
        {
            ConveyorSlot slot = SlotQueue.Dequeue();
            slot.transform.localPosition += new Vector3((ItemsMovedPerSecond / SlotQueue.Count) * Time.deltaTime, 0f, 0f);
            SlotQueue.Enqueue(slot);
        }

        ConveyorSlot _frontSlot = SlotQueue.Peek();
        if (_frontSlot.transform.localPosition.x <= .5f) { return; }

        if (_frontSlot.IsNotEmpty())
        {
            if (NextConveyor) // item at front moving to next belt
            {
                NextConveyor.PlaceItem(_frontSlot.GetItem());
                _frontSlot.Clear();
            }
            else // item stuck at front with no next belt
            {
                ShuffleAllItemsBackOne();
            }
        }

        _frontSlot.transform.localPosition -= new Vector3(1f, 0f, 0f);
        SlotQueue.Dequeue();
        SlotQueue.Enqueue(_frontSlot);
    }

    public void PlaceItem(Item _item)
    {
        // shuffle all but last slot
        for (int i = 0; i < SlotCount - 1; i++)
        {
            ConveyorSlot slotToShuffle = SlotQueue.Dequeue();
            SlotQueue.Enqueue(slotToShuffle);
        }

        // add item to last slot
        ConveyorSlot lastSlot = SlotQueue.Dequeue();
        lastSlot.SetItem(_item);

        RefreshConveyorConnections();

        SlotQueue.Enqueue(lastSlot);
    }

    public void RefreshConveyorConnections()
    {
        NextConveyor = null;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Utils.DirToVector(Facing), Vector2.up);
        if (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer)
        {
            NextConveyor = hit.transform.GetComponent<Conveyor>();
        }
    }

    private void ShuffleAllItemsBackOne()
    {
        ConveyorSlot slotCurr;
        ConveyorSlot slotNext;
        Item itemCurr;
        Item itemNext;


        slotCurr = SlotQueue.Dequeue();
        itemCurr = slotCurr.GetItem();
        slotCurr.Clear();

        for (int i = SlotCount - 2; i >= 0; i--)
        {
            slotNext = SlotQueue.Dequeue();
            itemNext = slotNext.GetItem();
            slotNext.Clear();

            slotNext.SetItem(itemCurr);
            SlotQueue.Enqueue(slotCurr);

            itemCurr = itemNext;
            slotCurr = slotNext;
        }

        SlotQueue.Enqueue(slotCurr);
    }

    // ROTATION

    public void RotateClockwise()
    {
        int currDir = (int) Facing;
        int newDir = (currDir + 1) % 4;

        Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, -90));

        conveyorManager.RefreshConveyorConnectionsAroundWorldPos(cellPos);
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
            ConveyorSlot slotToShuffle = SlotQueue.Dequeue();
            SlotQueue.Enqueue(slotToShuffle);
        }

        // add item to last slot
        ConveyorSlot lastSlot = SlotQueue.Dequeue();
        if (lastSlot.IsEmpty())
        {
            _receivable = true;
        }
        SlotQueue.Enqueue(lastSlot);

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
        SlotQueue = new Queue<ConveyorSlot>(SlotCount);

        ConveyorSlot[] _slots = GetComponentsInChildren<ConveyorSlot>();
        for (int i = SlotCount - 1; i >= 0; i--)
        {
            SlotQueue.Enqueue(_slots[i])   ;
        }
    }

    private void ToggleSlotSprites()
    {
        if (showSlotGraphics)
        {
            foreach (ConveyorSlot slot in SlotQueue)
            {
                slot.ShowSprite();
            }
        }
        else
        {
            foreach (ConveyorSlot slot in SlotQueue)
            {
                slot.HideSprite();
            }
        }
    }

}
