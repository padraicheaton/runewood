using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SaveData
{
    public SerializableInventoryDictionary chestDictionary;
    public PlayerData playerData;

    public InventorySaveData playerInventory;

    public CraftedSpellData craftedSpellData;

    public SerializablePlacedItemDictionary placedItemDictionary;

    public SaveData()
    {
        chestDictionary = new SerializableInventoryDictionary();
        playerData = new PlayerData();
        playerInventory = new InventorySaveData();
        craftedSpellData = new CraftedSpellData();
        placedItemDictionary = new SerializablePlacedItemDictionary();
    }
}
