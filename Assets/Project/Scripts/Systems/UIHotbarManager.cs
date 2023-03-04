using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIHotbarManager : MonoBehaviour
{

    [SerializeField] private Canvas hotbar;

    [SerializeField] private UIHotbarSlot[] slots;

    public int selectedSlot = 0;

    private void Awake()
    {
        slots = hotbar.GetComponentsInChildren<UIHotbarSlot>();
    }

    private void Start()
    {
        foreach (UIHotbarSlot slot in slots)
        {
            slot.Refresh();
        }
    }

    public void SelectSlot(int selection)
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

}
