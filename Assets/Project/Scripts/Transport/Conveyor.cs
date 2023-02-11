using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Conveyor : MonoBehaviour
{
    public float ItemsMovedPerSecond;
    public CardinalDirection Facing = CardinalDirection.East;

    public ConveyorSlot[] Slots;

    private float timer = 0f;
    private float timerProgress;

    private void FixedUpdate()
    {
        IncrementTimer();
        MoveItemsAlongConveyor();

        if (CycleComplete())
        {
            timer -= 1 / ItemsMovedPerSecond;
            ShiftItemsToNextSlot();
        }
    }

    // TIMING

    private void IncrementTimer()
    {
        timer += Time.fixedDeltaTime;
        timerProgress = timer / (1 / ItemsMovedPerSecond);
    }

    private bool CycleComplete()
    {
        return timer >= 1 / ItemsMovedPerSecond;
    }

    // ITEM MOVEMENT

    public void PlaceItem(GameObject _itemObj)
    {
        // _itemObj.transform.position = Slots[0].transform.position;
        Slots[0].SetItemObject(_itemObj, Slots[1].transform.position);
    }

    private void MoveItemsAlongConveyor()
    {
        foreach (ConveyorSlot slot in Slots)
        {
            if (slot.IsEmpty()) { continue; }

            GameObject itemObj = slot.GetItemObject();
            Item item = itemObj.GetComponent<Item>();
            
            if (item.IsAtTarget())
            {
                item.blockedFromMoving = true;
            }

            itemObj.transform.position = Vector3.Lerp(item.StartPos, item.TargetPos, timerProgress);
        }
    }

    private void ShiftItemsToNextSlot()
    {
        for (int i = Slots.Length - 1; i >= 0; i--)
        {
            ConveyorSlot currentSlot = Slots[i];
            if (currentSlot.IsEmpty()) { continue; }

            int oneSlotAhead = (i + 1) % Slots.Length;
            int twoSlotAhead = (i + 2) % Slots.Length;

            if (Slots[oneSlotAhead].IsNotEmpty()) { continue; }

            Slots[oneSlotAhead].SetItemObject(currentSlot.GetItemObject(), Slots[twoSlotAhead].transform.position);           
            currentSlot.Clear();
        }
    }

    // ROTATION

    public void RotateClockwise()
    {
        int currDir = (int) Facing;
        int newDir = (currDir + 1) % 4;

        Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, -90));
    }

    public void RotateAntiClockwise()
    {
        int currDir = (int) Facing;
        int newDir = (currDir - 1) % 4;

        Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, 90));
    }

}
