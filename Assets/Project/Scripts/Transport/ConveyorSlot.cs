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

    public bool IsEmpty() => itemObj == null;
    public bool IsNotEmpty() => itemObj != null;
    public Item GetItem() => item;

    public void SetItem(Item _item)
    {
        if (!_item) { return; }

        item = _item;
        itemObj = _item.gameObject;

        itemObj.transform.position = transform.position;
        itemObj.transform.SetParent(transform);
    }

    public void ClearItem()
    {
        itemObj = null;
        item = null;
    }

    // DRAWING

    public void ShowSprite() => _sr.enabled = true;
    public void HideSprite() => _sr.enabled = false;

}
