using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSlot : MonoBehaviour
{
    [SerializeField] private GameObject itemObj;
    [SerializeField] private Item item;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public bool IsEmpty()
    {
        return itemObj == null;
    }

    public void Clear()
    {
        itemObj = null;
        item = null;
    }

    public Item GetItem() => item;
    public GameObject GetItemObject() => itemObj;

    public void SetItemObject(Item _item)
    {
        itemObj = _item.gameObject;
        item = _item;

        itemObj.transform.position = transform.position;
        itemObj.transform.SetParent(transform);
    }

    public void ShowSprite() => _sr.enabled = true;
    public void HideSprite() => _sr.enabled = false;

}
