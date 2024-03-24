using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

public class PlayerInventoryHolder : InventoryHolder
{
    [SerializeField] private int hotbarSize;

    public static UnityAction OnPlayerInventoryChanged;
    public static UnityAction<InventorySlot> OnConsumableItemUsed;
    public static UnityAction<InventorySystem, string, int> OnBackpackInventoryDisplayRequested;

    public static UnityAction<InventoryItemData, int> OnAddToPlayerInventoryRequested;

    protected override void Awake()
    {
        base.Awake();

        OnConsumableItemUsed += RemoveItemFromInventory;

        InputProvider.onBackpackButtonPressed += OpenBackpack;

        OnDynamicInventoryDisplayRequested += (system, title, offst) => OpenBackpack();
        CraftingInventoryHolder.OnCraftingWindowOpened += OpenBackpack;

        OnAddToPlayerInventoryRequested += (item, amt) => AddToInventory(item, amt);
    }

    private void Start()
    {
        SaveGameManager.CurrentSaveData.playerInventory = new InventorySaveData(inventorySystem);
    }

    private void OnDestroy()
    {
        InputProvider.onBackpackButtonPressed -= OpenBackpack;
    }

    private void OpenBackpack()
    {
        OnBackpackInventoryDisplayRequested?.Invoke(inventorySystem, "Backpack", hotbarSize);
    }

    private void RemoveItemFromInventory(InventorySlot inventorySlotUsed)
    {
        inventorySystem.RemoveFromInventorySlot(inventorySlotUsed, 1);
    }

    public bool AddToInventory(InventoryItemData data, int amount)
    {
        if (inventorySystem.AddToInventory(data, amount))
            return true;

        return false;
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.playerInventory.invSystem != null)
        {
            inventorySystem = data.playerInventory.invSystem;
            OnPlayerInventoryChanged?.Invoke();
        }
    }
}