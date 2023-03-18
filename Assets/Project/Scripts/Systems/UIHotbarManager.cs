using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIHotbarManager : Singleton<UIHotbarManager>
{
    [SerializeField] private Canvas hotbar;
    [SerializeField] private UIHotbarSlot[] slots;

    public int selectedSlot = 0;

    private void Start()
    {
        hotbar = FindObjectOfType<UIHotbarSlot>().transform.parent.GetComponent<Canvas>();
        slots = hotbar.GetComponentsInChildren<UIHotbarSlot>();

        foreach (UIHotbarSlot slot in slots)
        {
            slot.Refresh();
        }
    }

    private void SelectSlot(int selection)
    {
        if (selection < 0 || selection > 9) { return; }

        selectedSlot = selection;

        for (int i = 0; i < slots.Length; i++)
        {
            if (i == selection) { slots[selection].Select();}
            if (i != selection) { slots[selection].Deselect(); }
        }
    }

    public UIHotbarSlot GetSelected()
    {
        return slots[selectedSlot];
    }

    public void OnHotbarSelectionEvent(int num)
    {
        SelectSlot(num);
    }

}
