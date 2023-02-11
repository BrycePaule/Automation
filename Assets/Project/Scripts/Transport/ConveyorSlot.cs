using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSlot : MonoBehaviour
{
    [SerializeField] private GameObject itemObj;
    [SerializeField] private Item item;

    public bool IsEmpty()
    {
        return itemObj == null;
    }

    public bool IsNotEmpty()
    {
        return itemObj != null;
    }

    public void Clear()
    {
        itemObj = null;
        item = null;
    }

    public Item GetItem() => item;
    public GameObject GetItemObject() => itemObj;

    public void SetItemObject(Item _item, Vector3 target)
    {
        itemObj = _item.gameObject;
        item = _item;

        item.StartPos = item.transform.position;
        item.TargetPos = target;

        itemObj.transform.SetParent(transform);
    }
}
