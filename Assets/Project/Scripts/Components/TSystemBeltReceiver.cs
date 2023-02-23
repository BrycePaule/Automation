using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TSystemBeltReceiver : MonoBehaviour, ITSystemReceivable
{
    public int MaxItems;
    public List<Item> Items;

    private Vector3 resetPos;
    public float gapWidth;

    private void Awake()
    {
        Items = new List<Item>();
        SetSpacing();
    }

    public bool CanReceive(Item _item)
    {
        if (!ItemMatchesFilter(_item)) { return false; }
        if (IsFull()) { return false; }

        return true;
    }

    public void Give(Item _item)
    {
        _item.transform.SetParent(transform);
        _item.transform.position = transform.position;
        _item.transform.localPosition += resetPos;

        Items.Add(_item);
    }

    public bool ItemMatchesFilter(Item _item)
    {
        TSystemReceiptFilter _filter = transform.GetComponent<TSystemReceiptFilter>();
        if (_filter == null) { return true; }
    
        return _filter.Check(_item);
    }

    // HELPERS

    private bool IsFull()
    {
        return Items.Count >= MaxItems;
    }

    private Item RearItem()
    {
        return Items[Items.Count - 1];
    }

    private void SetSpacing()
    {
        resetPos = new Vector3(-0.5f, 0f, 0f);
        gapWidth =  1f / (MaxItems + 1);
    }
    
}
