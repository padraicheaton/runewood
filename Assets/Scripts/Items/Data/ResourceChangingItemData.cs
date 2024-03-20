using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Resource Changing Item")]
public class ResourceChangingItemData : ConsumableItemData
{
    [Header("Resource Changing Settings")]
    public PlayerCombat.PlayerResources resource;
    public float amount;
}
