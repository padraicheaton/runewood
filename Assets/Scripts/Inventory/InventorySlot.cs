using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// TODO: Abstract the deserialisation method away from this class, so it does not load from resources constantly

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private int itemID;
    [SerializeField] private int stackSize;

    public InventoryItemData Data => ItemManager.GetItem(itemID);
    public int StackSize => stackSize;

    public InventorySlot(InventoryItemData item, int amount)
    {
        UpdateInventorySlot(item, itemID, amount);
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        UpdateInventorySlot(null, -1, -1);
    }

    public void AssignItem(InventorySlot slot)
    {
        if (Data == slot.Data)
            AddToStack(slot.stackSize);
        else
            UpdateInventorySlot(slot.Data, slot.Data.ID, slot.stackSize);
    }

    public void UpdateInventorySlot(InventoryItemData item, int id, int amount)
    {
        itemID = id;
        stackSize = amount;
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

        splitStackSlot = new InventorySlot(Data, halfStack);

        return true;
    }
}
