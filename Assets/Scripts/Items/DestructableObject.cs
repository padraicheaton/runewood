using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(HealthComponent))]
public class DestructableObject : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private ItemDropLootTable lootTable;

    private HealthComponent healthComponent;

    private void Start()
    {
        healthComponent = GetComponent<HealthComponent>();

        healthComponent.onResourceDepleted += OnObjectDestroyed;
    }

    private void OnObjectDestroyed()
    {
        foreach (InventoryItemData item in lootTable.GetLoot())
            ItemGenerator.Instance.GenerateItem(item, transform.position);

        Destroy(gameObject);
    }
}
