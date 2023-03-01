using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public PrefabType building;

    [SerializeField] private Image itemIcon;
    [SerializeField] private Image selectionRing;

    private bool selected;

    private void OnEnable()
    {
        Deselect();
    }

    public void Select()
    {
        selected = true;
        selectionRing.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        selected = false;
        selectionRing.gameObject.SetActive(false);
    }

    public void SetItemIcon(Sprite icon)
    {
        itemIcon.gameObject.SetActive(true);
        itemIcon.sprite = icon;
    }

    public void Refresh()
    {
        if (selected)
        {
            Select();
        }
        else
        {
            Deselect();
        }
    }
}
