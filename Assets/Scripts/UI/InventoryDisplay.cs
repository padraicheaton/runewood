using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

public abstract class InventoryDisplay : MonoBehaviour
{
    protected InventorySystem inventorySystem;
    protected Dictionary<InventorySlot_UI, InventorySlot> slotDictionary;

    public InventorySystem InventorySystem => inventorySystem;
    public Dictionary<InventorySlot_UI, InventorySlot> SlotDictionary => slotDictionary;

    protected virtual void Start()
    {

    }

    public abstract void AssignSlots(InventorySystem invToDisplay, int offset);

    protected virtual void UpdateSlot(InventorySlot updatedSlot)
    {
        foreach (KeyValuePair<InventorySlot_UI, InventorySlot> slot in slotDictionary)
        {
            if (slot.Value == updatedSlot)
            {
                slot.Key.UpdateUISlot(updatedSlot);
            }
        }
    }

    public void SlotClicked(InventorySlot_UI clickedUISlot, bool wasRightClicked)
    {
        if (clickedUISlot.AssignedInventorySlot.Data != null && MouseItemData.Instance.InventorySlot.Data == null) // If the clicked slot has an item in it and the mouse does not
        {
            // If holding the split item button and can split the stack, pick up the split stack
            if (wasRightClicked && clickedUISlot.AssignedInventorySlot.SplitStack(out InventorySlot splitStackSlot))
            {
                clickedUISlot.UpdateUISlot();

                MouseItemData.Instance.UpdateMouseSlot(splitStackSlot);

                return;
            }
            else
            {
                // else Pick up the item

                MouseItemData.Instance.UpdateMouseSlot(clickedUISlot.AssignedInventorySlot);
                clickedUISlot.ClearSlot();

                return;
            }
        }

        if (clickedUISlot.AssignedInventorySlot.Data == null && MouseItemData.Instance.InventorySlot.Data != null) // If the clicked slot has no item, but the mouse does
        {
            if (wasRightClicked)
            {
                // Only place one item in there
                clickedUISlot.AssignedInventorySlot.UpdateInventorySlot(MouseItemData.Instance.InventorySlot.Data.ID, 1);
                clickedUISlot.UpdateUISlot();

                MouseItemData.Instance.InventorySlot.RemoveFromStack(1);

                if (MouseItemData.Instance.InventorySlot.StackSize <= 0)
                    MouseItemData.Instance.ClearSlot();
                else
                    MouseItemData.Instance.UpdateMouseSlot();
            }
            else
            {
                // Place the item in there
                clickedUISlot.AssignedInventorySlot.AssignItem(MouseItemData.Instance.InventorySlot);
                clickedUISlot.UpdateUISlot();
                MouseItemData.Instance.ClearSlot();
            }

            return;
        }

        if (clickedUISlot.AssignedInventorySlot.Data != null && MouseItemData.Instance.InventorySlot.Data != null) // If both have an item
        {
            if (clickedUISlot.AssignedInventorySlot.Data != MouseItemData.Instance.InventorySlot.Data) // If they are different items
            {
                // Swap them
                SwapSlots(clickedUISlot);

                return;
            }
            else if (clickedUISlot.AssignedInventorySlot.RoomLeftInStack(MouseItemData.Instance.InventorySlot.StackSize)) // If the clicked slot has room for the mouse slot
            {
                // Try to combine them

                clickedUISlot.AssignedInventorySlot.AssignItem(MouseItemData.Instance.InventorySlot);
                clickedUISlot.UpdateUISlot();

                MouseItemData.Instance.ClearSlot();

                return;
            }
            else if (!clickedUISlot.AssignedInventorySlot.RoomLeftInStack(MouseItemData.Instance.InventorySlot.StackSize, out int amountRemaining))
            {
                if (amountRemaining < 1)
                {
                    SwapSlots(clickedUISlot);
                    return;
                }

                int remainingOnMouse = MouseItemData.Instance.InventorySlot.StackSize - amountRemaining;
                InventorySlot newSlot = new InventorySlot(MouseItemData.Instance.InventorySlot.Data.ID, remainingOnMouse);

                MouseItemData.Instance.ClearSlot();
                MouseItemData.Instance.UpdateMouseSlot(newSlot);

                clickedUISlot.AssignedInventorySlot.AddToStack(amountRemaining);
                clickedUISlot.UpdateUISlot();

                return;
            }
        }
    }

    private void SwapSlots(InventorySlot_UI slot)
    {
        InventorySlot temporary = new InventorySlot(MouseItemData.Instance.InventorySlot.Data.ID, MouseItemData.Instance.InventorySlot.StackSize);
        MouseItemData.Instance.ClearSlot();

        MouseItemData.Instance.UpdateMouseSlot(slot.AssignedInventorySlot);

        slot.ClearSlot();
        slot.AssignedInventorySlot.AssignItem(temporary);
        slot.UpdateUISlot();
    }
}
