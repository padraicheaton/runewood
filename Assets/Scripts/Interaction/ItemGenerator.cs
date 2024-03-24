using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGenerator : MonoBehaviour, IInteractable
{
    [Header("Pickup")]
    [SerializeField] private ItemPickup itemPickupPrefab;
    [SerializeField] private List<InventoryItemData> items;

    private int index;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public virtual void Interact(Interactor interactor, out bool interactionSuccessful)
    {
        GenerateItem(items[index]);

        index++;

        if (index >= items.Count)
            index = 0;

        interactionSuccessful = true;
    }

    protected void GenerateItem(InventoryItemData itemToGenerate)
    {
        ItemPickup createdPickup = Instantiate(itemPickupPrefab, transform.position + Vector3.back, Quaternion.identity);

        createdPickup.Init(itemToGenerate);
    }

    public void EndInteraction()
    {

    }
}
