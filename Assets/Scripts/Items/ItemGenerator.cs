using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGenerator : Singleton<ItemGenerator>
{
    [Header("Pickup")]
    [SerializeField] private ItemPickup itemPickupPrefab;

    public void GenerateItem(InventoryItemData itemToGenerate, Vector3 position)
    {
        ItemPickup createdPickup = Instantiate(itemPickupPrefab, position, Quaternion.identity);

        createdPickup.Init(itemToGenerate);
    }
}
