using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ChestData : PlaceableItem
{
    private ChestInventory chestInventory;

    public override void Initialise(PlaceableItemData itemData)
    {
        this.itemData = itemData;

        if (chestInventory == null)
            chestInventory = GetComponent<ChestInventory>();

        // This is only called when placed for the first time

        AreaManager.Instance.RecordPlacedChest(itemData, chestInventory.InventorySystem, transform);
    }

    public void TryLoadInventory(PlaceableItemSaveData chestSaveData)
    {
        if (chestInventory == null)
            chestInventory = GetComponent<ChestInventory>();

        chestInventory.LoadInventoryFromSave(chestSaveData.inventorySystem);
    }
}