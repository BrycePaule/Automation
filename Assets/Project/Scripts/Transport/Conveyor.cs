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

    public Conveyor PushingTo;
    public CardinalDirection Facing = CardinalDirection.East;
    public Queue<ConveyorSlot> SlotQueue;

    [Header("References")]
    [SerializeField] private ConveyorManager conveyorManager;
    [SerializeField] private GameObject slotPrefab;

    [Header("Debug")]
    [SerializeField] private bool showSlotGraphics;

    private void Awake()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            Vector3 _positionOffset = new Vector3(-0.5f + ((1f/SlotCount) * (i + 1f)), 0f, 0f);
            GameObject slotObj = Instantiate(slotPrefab, transform.position + _positionOffset, Quaternion.identity);
            slotObj.transform.parent = transform;
            slotObj.name = slotPrefab.name + "_" + i;
            slotObj.transform.localScale = new Vector3(
                slotObj.transform.localScale.x /  SlotCount,
                slotObj.transform.localScale.y,
                slotObj.transform.localScale.z);
        }

        SlotQueue = new Queue<ConveyorSlot>(SlotCount);
        ConveyorSlot[] slots = GetComponentsInChildren<ConveyorSlot>();
        for (int i = SlotCount - 1; i >= 0; i--)
        {
            SlotQueue.Enqueue(slots[i]);
        }
    }

    private void FixedUpdate()
    {
        ToggleSlotSprites();
        MoveSlotsAlongConveyor();
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

    // ITEM MOVEMENT

    private void MoveSlotsAlongConveyor()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            ConveyorSlot slot = SlotQueue.Dequeue();
            slot.transform.localPosition += new Vector3((ItemsMovedPerSecond / SlotQueue.Count) * Time.deltaTime, 0f, 0f);
            SlotQueue.Enqueue(slot);
        }

        ConveyorSlot first = SlotQueue.Peek();

        if (first.transform.localPosition.x >= .5f)
        {
            first = SlotQueue.Dequeue();

            if (!first.IsEmpty() && PushingTo)
            {
                PushingTo.PlaceItem(first.GetItem());
                first.Clear();
            }

            first.transform.localPosition -= new Vector3(1f, 0f, 0f);
            SlotQueue.Enqueue(first);
        }
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
        lastSlot.SetItemObject(_item);

        RefreshConveyorConnections();

        SlotQueue.Enqueue(lastSlot);
    }

    public void RefreshConveyorConnections()
    {
        PushingTo = null;

        RaycastHit2D hit = Physics2D.Raycast(transform.position + Utils.DirToVector(Facing), Vector2.up);
        if (hit && hit.transform.gameObject.layer == (int) Layers.Conveyer)
        {
            PushingTo = hit.transform.GetComponent<Conveyor>();
        }
    }

    // ROTATION

    public void RotateClockwise()
    {
        int currDir = (int) Facing;
        int newDir = (currDir + 1) % 4;

        Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, -90));

        RefreshConveyorConnections();
    }

    // HELPERS

    public void SetReferences(ConveyorManager _convManager)
    {
        conveyorManager = _convManager;
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
}
