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
    public Tilemap Tilemap;
    [SerializeField] private GameObject SlotPrefab;

    [Header("Debug")]
    [SerializeField] private bool ShowSlotGraphics;

    private void Awake()
    {
        for (int i = 0; i < SlotCount; i++)
        {
            Vector3 _positionOffset = new Vector3(-0.5f + ((1f/SlotCount) * (i + 1f)), 0f, 0f);
            GameObject slotObj = Instantiate(SlotPrefab, transform.position + _positionOffset, Quaternion.identity);
            slotObj.transform.parent = transform;
            slotObj.name = SlotPrefab.name + "_" + i;
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
        MoveSlotsAlongConveyor();
        DrawSlotSprites();
    }

    private void DrawSlotSprites()
    {
        if (ShowSlotGraphics)
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

    // ITEM MOVEMENT

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

        RefreshConnections();

        SlotQueue.Enqueue(lastSlot);
    }

    public void RefreshConnections()
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

        RefreshConnections();
    }
}
