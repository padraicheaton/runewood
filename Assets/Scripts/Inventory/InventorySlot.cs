using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySlot
{
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int stackSize;

    public InventoryItemData Data => itemData;
    public int StackSize => stackSize;

    public InventorySlot(InventoryItemData item, int amount)
    {
        UpdateInventorySlot(item, amount);
    }

    public InventorySlot()
    {
        ClearSlot();
    }

    public void ClearSlot()
    {
        UpdateInventorySlot(null, -1);
    }

    public void AssignItem(InventorySlot slot)
    {
        if (itemData == slot.Data)
            AddToStack(slot.stackSize);
        else
            UpdateInventorySlot(slot.Data, slot.stackSize);
    }

    public void UpdateInventorySlot(InventoryItemData item, int amount)
    {
        itemData = item;
        stackSize = amount;
    }

    public bool RoomLeftInStack(int amountToAdd, out int amountRemaining)
    {
        amountRemaining = itemData.maximumStackSize - stackSize;

        return RoomLeftInStack(amountToAdd);
    }

    public bool RoomLeftInStack(int amountToAdd)
    {
        return stackSize + amountToAdd <= itemData.maximumStackSize;
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

        splitStackSlot = new InventorySlot(itemData, halfStack);

        return true;
    }
}
