using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Conveyor : MonoBehaviour
{
    public float ItemsMovedPerSecond;
    public CardinalDirection Facing = CardinalDirection.East;

    [Header("References")]
    public Conveyor Provider;
    public Conveyor Receiver;
    public ConveyorSlot[] Slots;
    [SerializeField] private Tilemap Tilemap;


    private float timerCount = 0f;
    private float timerProgress;

    private void FixedUpdate()
    {
        timerCount += Time.fixedDeltaTime;
        timerProgress = timerCount / (1 / ItemsMovedPerSecond);

        MoveItemsAlongConveyor();

        if (CycleComplete())
        {
            ResetTimer();
        }
    }

    // TIMING

    private void ResetTimer() => timerCount -= 1 / ItemsMovedPerSecond;
    private bool CycleComplete() => timerCount >= 1 / ItemsMovedPerSecond;

    // ITEM MOVEMENT

    public void PlaceItem(Item _item)
    {
        _item.transform.position = Slots[0].transform.position;
        Slots[0].SetItemObject(_item, Slots[1].transform.position);
    }

    private void MoveItemsAlongConveyor()
    {
        for (int currSlot = Slots.Length - 1; currSlot >= 0; currSlot--)
        {
            if (Slots[currSlot].IsEmpty()) { continue; }

            Item _item = Slots[currSlot].GetItemObject().GetComponent<Item>();
            int oneSlotAhead = (currSlot + 1) % Slots.Length;
            int twoSlotAhead = (currSlot + 2) % Slots.Length;

            if (_item.IsAtTarget() && Slots[oneSlotAhead].IsEmpty())
            {
                Slots[oneSlotAhead].SetItemObject(_item, Slots[twoSlotAhead].transform.position);           
                Slots[currSlot].Clear();
            }

            if (Slots[oneSlotAhead].IsNotEmpty())
            {
                _item.transform.position = _item.StartPos;
            }

            if (Slots[oneSlotAhead].IsEmpty())
            {
                _item.transform.position = Vector3.Lerp(_item.StartPos, _item.TargetPos, timerProgress);
            }
        }
    }

    // ROTATION

    public void RotateClockwise()
    {
        int currDir = (int) Facing;
        int newDir = (currDir + 1) % 4;

        Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, -90));

        for (int currSlot = 0; currSlot < Slots.Length; currSlot++)
        {
            if (Slots[currSlot].IsEmpty()) { continue; }

            int oneSlotAhead = (currSlot + 1) % Slots.Length;
            Slots[currSlot].SetItemObject(Slots[currSlot].GetItem(), Slots[oneSlotAhead].transform.position);
        }
    }

    public void RotateAntiClockwise()
    {
        int currDir = (int) Facing;
        int newDir = (currDir - 1) % 4;

        Facing = (CardinalDirection) newDir;
        transform.Rotate(new Vector3(0, 0, 90));

        for (int currSlot = 0; currSlot < Slots.Length; currSlot++)
        {
            if (Slots[currSlot].IsEmpty()) { continue; }

            int oneSlotAhead = (currSlot + 1) % Slots.Length;
            Slots[currSlot].SetItemObject(Slots[currSlot].GetItem(), Slots[oneSlotAhead].transform.position);
        }
    }

}
