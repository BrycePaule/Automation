using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HotbarManager : MonoBehaviour
{

    [SerializeField] private Canvas hotbar;

    [SerializeField] private HotbarSlot[] slots;

    public int selectedSlot = 0;

    private void Start()
    {
        slots = hotbar.GetComponentsInChildren<HotbarSlot>();
        foreach (HotbarSlot slot in slots)
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
            if (i == selection) { slots[selection].Select(); }
            if (i != selection) { slots[selection].Deselect(); }
        }
    }

    public HotbarSlot GetSelected()
    {
        return slots[selectedSlot];
    }

}
