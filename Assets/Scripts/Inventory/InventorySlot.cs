using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Abstract the deserialisation method away from this class, so it does not load from resources constantly

[System.Serializable]
public class InventorySlot
{
    [Header("General Data")]
    [SerializeField] private int itemID;
    [SerializeField] private int stackSize;

    public InventoryItemData Data => ItemManager.GetItem(itemID);
    public int StackSize => stackSize;

    // Spell Specific Data
    [Header("Spell Specific Data")]
    [SerializeField] private SpellComponentData.Element element;
    [SerializeField] private List<SpellComponentData.Action> actions;

    public InventorySlot(int itemID, int amount)
    {
        UpdateInventorySlot(itemID, amount);
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        UpdateInventorySlot(-1, -1);
    }

    public void AssignItem(InventorySlot slot)
    {
        if (Data == slot.Data)
            AddToStack(slot.stackSize);
        else
            UpdateInventorySlot(slot.Data.ID, slot.stackSize);
    }

    public void UpdateInventorySlot(int id, int amount)
    {
        itemID = id;
        stackSize = amount;

        // Save data if containing spell
        if (itemID != -1 && Data.GetType() == typeof(SpellItemData))
        {
            SpellItemData spellItemData = Data as SpellItemData;

            // If this slot has already loaded in elements and actions, apply them to this spell
            if (element != SpellComponentData.Element.Null && actions != null)
            {
                spellItemData.element = element;
                spellItemData.actions = actions;
            }
            else
            {
                element = (Data as SpellItemData).element;
                actions = (Data as SpellItemData).actions;
            }
        }
        else
        {
            element = SpellComponentData.Element.Null;
            actions = null;
        }
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = Data.maximumStackSize - stackSize;

        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        return stackSize + amountToAdd <= Data.maximumStackSize;
    }

    public void AddToStack(int amount)
    {
        stackSize += amount;
    }

    public void RemoveFromStack(int amount)
    {
        stackSize -= amount;
    }

    public bool SplitStack(out InventorySlot splitStackSlot)
    {
        splitStackSlot = null;

        if (stackSize <= 1)
            return false;

        int halfStack = Mathf.RoundToInt(stackSize / 2);

        RemoveFromStack(halfStack);

        splitStackSlot = new InventorySlot(itemID, halfStack);

        return true;
    }
}
