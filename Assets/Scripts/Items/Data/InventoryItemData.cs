using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Item")]
public class InventoryItemData : ScriptableObject
{
    [ReadOnly] public int ID = -1;
    public Sprite icon;
    public int maximumStackSize;
    public string displayName;
    [TextArea(4, 4)] public string description;

    public virtual string GetDisplayName() => displayName;
    public virtual string GetDescription() => description;
}
