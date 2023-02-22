using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorSlot : MonoBehaviour
{
    [SerializeField] private GameObject itemObj;
    [SerializeField] private Item item;

    private SpriteRenderer _sr;

    private bool Empty;

    private void Awake()
    {
        _sr = GetComponent<SpriteRenderer>();
    }

    public bool IsEmpty() => item == null;
    public bool IsNotEmpty() => item != null;
    public Item GetItem() => item;

    private void Update()
    {
        Empty = IsEmpty();
    }

    public void SetItem(Item _item)
    {
        if (_item == null) { return; }

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
