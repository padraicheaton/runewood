using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    [ReadOnly] public int ID = -1;
    public Sprite icon;
    public int maximumStackSize;
}
