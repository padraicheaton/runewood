using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ItemGenerator : MonoBehaviour, IInteractable
{
    [Header("Pickup")]
    [SerializeField] private ItemPickup itemPickupPrefab;
    [SerializeField] private InventoryItemData item;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    public void Interact(Interactor interactor, out bool interactionSuccessful)
    {
        ItemPickup createdPickup = Instantiate(itemPickupPrefab, transform.position + Vector3.back, Quaternion.identity);

        createdPickup.Init(item);

        interactionSuccessful = true;
    }

    public void EndInteraction()
    {

    }
}
