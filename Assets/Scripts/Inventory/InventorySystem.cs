using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using System.Linq;

[System.Serializable]
public class InventorySystem
{
    [SerializeField] private List<InventorySlot> inventorySlots;

    public List<InventorySlot> InventorySlots => inventorySlots;
    public int InventorySize => inventorySlots.Count;

    public UnityAction<InventorySlot> OnInventorySlotChanged;

    public InventorySystem(int size)
    {
        inventorySlots = new List<InventorySlot>(size);

        for (int i = 0; i < size; i++)
            inventorySlots.Add(new InventorySlot());
    }

    public bool AddToInventory(InventoryItemData item, int amount)
    {
        if (ContainsItem(item, out List<InventorySlot> invSlots))
        {
            foreach (InventorySlot slot in invSlots)
            {
                if (slot.RoomLeftInStack(amount))
                {
                    slot.AddToStack(amount);

                    OnInventorySlotChanged?.Invoke(slot);

                    return true;
                }
            }
        }

        if (HasFreeSlot(out InventorySlot freeSlot))
        {
            freeSlot.UpdateInventorySlot(item, amount);

            OnInventorySlotChanged?.Invoke(freeSlot);

            return true;
        }

        return false;
    }

    public bool RemoveFromInventorySlot(InventorySlot itemSlot, int amount)
    {
        if (inventorySlots.Contains(itemSlot) && itemSlot.StackSize >= amount)
        {
            itemSlot.RemoveFromStack(amount);

            if (itemSlot.StackSize <= 0)
                itemSlot.ClearSlot();

            return true;
        }

        return false;
    }

    public bool ContainsItem(InventoryItemData itemToAdd, out List<InventorySlot> invSlots)
    {
        invSlots = InventorySlots.Where(slot => slot.Data == itemToAdd).ToList();

        return invSlots != null && invSlots.Count >= 1;
    }

    public bool HasFreeSlot(out InventorySlot freeSlot)
    {
        freeSlot = InventorySlots.FirstOrDefault(slot => slot.Data == null);

        return freeSlot != null;
    }
}
