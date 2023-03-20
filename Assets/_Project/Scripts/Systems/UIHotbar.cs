using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace bpdev
{
    public class UIHotbar : Singleton<UIHotbar>
    {
        private int selected = 0;

        private Canvas hotbar;
        private UIHotbarSlot[] slots;

        protected override void Awake()
        {
            base.Awake();

            hotbar = GetComponent<Canvas>();
            slots = GetComponentsInChildren<UIHotbarSlot>();
        }

        private void OnEnable()
        {
            RefreshAllSlots();
        }

        private void RefreshAllSlots()
        {
            foreach (UIHotbarSlot slot in slots)
            {
                slot.Refresh();
            }
        }

        // GETTER

        public UIHotbarSlot GetSelected()
        {
            return slots[selected];
        }

        // EVENT

        public void OnHotbarSelectionEvent(int num)
        {
            SelectSlot(num);
        }

        private void SelectSlot(int selectionNum)
        {
            if (selectionNum < 0 || selectionNum > 9) { return; }

            selected = selectionNum;

            for (int i = 0; i < slots.Length; i++)
            {
                if (i == selectionNum) { slots[selectionNum].Select();}
                if (i != selectionNum) { slots[selectionNum].Deselect(); }
            }
        }
    }
}