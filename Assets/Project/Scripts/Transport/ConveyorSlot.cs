using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSlot : MonoBehaviour
{
    public Item Item;

    private SpriteRenderer _sr;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public bool IsEmpty() => Item == null;
    public bool IsNotEmpty() => Item != null;

    public void SetItem(Item _item)
    {
        if (_item == null) { return; }

        Item = _item;

        Item.transform.position = transform.position;
        Item.transform.SetParent(transform);
    }

    public void ClearItem()
    {
        Item = null;
    }

    // DRAWING

    public void ShowSprite() => _sr.enabled = true;
    public void HideSprite() => _sr.enabled = false;

}
