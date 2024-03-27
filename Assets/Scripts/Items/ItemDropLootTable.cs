using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Loot Table")]
public class ItemDropLootTable : ScriptableObject
{
    public List<ItemDrop> drops;

    public List<InventoryItemData> GetLoot()
    {
        List<InventoryItemData> items = new List<InventoryItemData>();

        foreach (ItemDrop drop in drops)
        {
            if (drop.chance <= Random.value)
                items.Add(drop.item);
        }

        return items;
    }

    [System.Serializable]
    public struct ItemDrop
    {
        public InventoryItemData item;
        [Range(0f, 1f)] public float chance;
    }
}
