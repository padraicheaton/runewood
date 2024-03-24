using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class CraftingInventoryHolder : InventoryHolder, IInteractable
{
    [Header("References")]
    [SerializeField] private ModalWindow craftingModal;
    [SerializeField] private CraftingInventoryDisplay craftingInventoryDisplay;

    public static UnityAction OnCraftingWindowOpened;
    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private void Start()
    {
        craftingModal.OnModalWindowClosed += ReturnAnyItemsToPlayer;
    }

    private void OnDestroy()
    {
        craftingModal.OnModalWindowClosed -= ReturnAnyItemsToPlayer;
    }

    protected override void LoadInventory(SaveData data)
    {

    }

    public void Interact(Interactor interactor, out bool interactionSuccessful)
    {
        craftingModal.Show();
        OnCraftingWindowOpened?.Invoke();

        interactionSuccessful = true;
    }

    public void EndInteraction()
    {

    }

    private void ReturnAnyItemsToPlayer()
    {
        craftingInventoryDisplay.ReturnAnyItemsToPlayer();
    }
}
