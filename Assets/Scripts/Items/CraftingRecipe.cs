using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Crafting Recipe")]
public class CraftingRecipe : ScriptableObject
{
    [Tooltip("First Element is center of the table, the rest are around")]
    public List<InventoryItemData> ingredients;
    public InventoryItemData result;
}
