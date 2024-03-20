using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ItemManager
{
    private static Database ItemDatabase;

    public static Database GetDatabase()
    {
        if (ItemDatabase != null)
            return ItemDatabase;

        ItemDatabase = Resources.Load<Database>("Database");

        return ItemDatabase;
    }

    public static InventoryItemData GetItem(int id)
    {
        return GetDatabase().GetItem(id);
    }
}
