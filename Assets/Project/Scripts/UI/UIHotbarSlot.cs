using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHotbarSlot : MonoBehaviour
{
    public BuildingType buildingType;

    [Header("References")]
    [SerializeField] private Image itemIcon;
    [SerializeField] private Image selectionRing;

    private bool selected;

    private void Awake()
    {
        RefreshType();

        selected = false;
    }

    private void OnEnable()
    {
        Deselect();
    }

    public void Select()
    {
        selected = true;
        selectionRing.color = Utils.Colour.SetAlpha(selectionRing.color, 1);
        // selectionRing.gameObject.SetActive(true);
    }

    public void Deselect()
    {
        selected = false;
        selectionRing.color = Utils.Colour.SetAlpha(selectionRing.color, 0);
        // selectionRing.gameObject.SetActive(false);
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

    private void RefreshType()
    {
        if (buildingType == BuildingType.NULL) { return; }

        itemIcon.sprite = BuildingProxy.Instance.GetByType(buildingType).InventoryIcon;
    }
}
