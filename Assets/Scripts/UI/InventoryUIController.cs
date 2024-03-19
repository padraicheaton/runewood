using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUIController : MonoBehaviour
{
    [SerializeField] private DynamicInventoryDisplay dynamicInventoryPanel;
    [SerializeField] private ModalWindow dynamicInventoryPanelModal;

    [SerializeField] private DynamicInventoryDisplay backpackInventoryPanel;
    [SerializeField] private ModalWindow backpackInventoryPanelModal;

    private void OnEnable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested += DisplayInventory;
        PlayerInventoryHolder.OnBackpackInventoryDisplayRequested += DisplayBackpack;
    }

    private void OnDisable()
    {
        InventoryHolder.OnDynamicInventoryDisplayRequested -= DisplayInventory;
        PlayerInventoryHolder.OnBackpackInventoryDisplayRequested -= DisplayBackpack;
    }

    private void DisplayInventory(InventorySystem invToDisplay, string displayName, int offset)
    {
        dynamicInventoryPanelModal.Show();

        dynamicInventoryPanel.RefreshDynamicInventory(invToDisplay, displayName, offset);
    }

    private void DisplayBackpack(InventorySystem invToDisplay, string displayName, int offset)
    {
        if (dynamicInventoryPanelModal.shown)
            backpackInventoryPanelModal.Show();
        else
            backpackInventoryPanelModal.Toggle();

        backpackInventoryPanel.RefreshDynamicInventory(invToDisplay, displayName, offset);
    }
}
