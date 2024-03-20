using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class ConsumableItemData : UseableItemData
{
    public override void TryUseItem(InventorySlot inventorySlot)
    {
        // Reduce the stack count by one
        PlayerInventoryHolder.OnConsumableItemUsed?.Invoke(inventorySlot);

        // Use the item
        OnItemUsed();
    }

    protected override void OnItemUsed()
    {
        PlayerCombat.onConsumableItemUsed?.Invoke(this);
    }
}
