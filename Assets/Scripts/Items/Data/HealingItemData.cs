using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Healing Item")]
public class HealingItemData : ConsumableItemData
{
    [Header("Heal Settings")]
    [SerializeField] private float healAmount;

    protected override void OnItemUsed()
    {
        Debug.Log($"Should Heal the Player by: {healAmount}");
    }
}
