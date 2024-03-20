using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Item Database")]
public class Database : ScriptableObject
{
    [SerializeField] private List<InventoryItemData> itemDatabase;

    [ContextMenu("Set IDs")]
    public void SetItemIDs()
    {
        itemDatabase = new List<InventoryItemData>();

        // Load All Items from Assets
        List<InventoryItemData> foundItems = Resources.LoadAll<InventoryItemData>("Items")
            .OrderBy(item => item.ID).ToList();

        // Include any crafted spells
        foundItems.AddRange(SaveGameManager.CurrentSaveData.craftedSpellData.GetAllSpellItems());
        foundItems.OrderBy(item => item.ID);

        // Triage them into different lists to be edited where necessary
        List<InventoryItemData> hasIDInRange = foundItems.Where(item => item.ID != -1 && item.ID < foundItems.Count)
            .OrderBy(item => item.ID).ToList();

        List<InventoryItemData> hasIDNotInRange = foundItems.Where(item => item.ID != -1 && item.ID >= foundItems.Count)
            .OrderBy(item => item.ID).ToList();

        List<InventoryItemData> hasNoID = foundItems.Where(item => item.ID == -1).ToList();

        // Enter all into the database correctly
        int index = 0;

        for (int i = 0; i < foundItems.Count; i++)
        {
            InventoryItemData itemToAdd;

            // Check if it already has a correct ID, then don't tamper with it
            itemToAdd = hasIDInRange.Find(data => data.ID == i);

            if (itemToAdd != null)
            {
                itemDatabase.Add(itemToAdd);
            }
            else if (index < hasNoID.Count)
            {
                hasNoID[index].ID = i;

                itemToAdd = hasNoID[index];

                index++;

                itemDatabase.Add(itemToAdd);
            }
        }

        foreach (InventoryItemData item in hasIDNotInRange)
        {
            itemDatabase.Add(item);
        }
    }

    public InventoryItemData GetItem(int id)
    {
        return itemDatabase.Find(item => item.ID == id);
    }

    public SpellItemData GetGenericSpellItem()
    {
        return itemDatabase.Find(item => item.GetType() == typeof(SpellItemData)) as SpellItemData;
    }

    public int GetNextAvailableID()
    {
        int id = 0;

        while (itemDatabase.Find(item => item.ID == id) != null)
            id++;

        return id;
    }

    public void AddCustomCraftedSpell(SpellItemData spellItemData)
    {
        spellItemData.ID = GetNextAvailableID();

        itemDatabase.Add(spellItemData);
    }
}
