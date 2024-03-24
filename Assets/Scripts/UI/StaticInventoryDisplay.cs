using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StaticInventoryDisplay : InventoryDisplay
{
    [SerializeField] private InventoryHolder inventoryHolder;
    [SerializeField] protected InventorySlot_UI[] slots;

    protected override void Start()
    {
        base.Start();

        RefreshStaticDisplay();
    }

    protected void RefreshStaticDisplay()
    {
        AssignInventorySystem(inventoryHolder.InventorySystem);
    }

    public void AssignInventorySystem(InventorySystem system)
    {
        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChanged -= UpdateSlot;

        inventorySystem = system;
        inventorySystem.OnInventorySlotChanged += UpdateSlot;

        AssignSlots(inventorySystem, 0);
    }

    public override void AssignSlots(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        for (int i = 0; i < slots.Length; i++)
        {
            slotDictionary.Add(slots[i], inventorySystem.InventorySlots[i]);
            slots[i].Init(inventorySystem.InventorySlots[i]);
        }
    }
}
