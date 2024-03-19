using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UseableItemData : InventoryItemData
{
    public virtual void TryUseItem(InventorySlot inventorySlot)
    {
        OnItemUsed();
    }
    protected abstract void OnItemUsed();
}
