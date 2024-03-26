using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
using Unity.Loading;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Events;

[RequireComponent(typeof(UniqueID))]
public class ChestInventory : InventoryHolder, IInteractable
{
    [Header("Visual Settings")]
    [SerializeField] private string displayName;

    public UnityAction<IInteractable> OnInteractionComplete { get; set; }

    private UniqueID uniqueID;

    protected override void Awake()
    {
        base.Awake();

        uniqueID = GetComponent<UniqueID>();
        LoadInventory(SaveGameManager.CurrentSaveData);
    }

    public void LoadInventoryFromSave(InventorySystem _inventorySystem)
    {
        inventorySystem = _inventorySystem;
    }

    protected override void LoadInventory(SaveData data)
    {
        // Debug.Log($"ID {uniqueID.ID}");

        // if (data.chestDictionary.TryGetValue(uniqueID.ID, out InventorySaveData chestSaveData))
        // {
        //     inventorySystem = chestSaveData.invSystem;
        //     transform.position = chestSaveData.position;
        //     transform.rotation = chestSaveData.rotation;
        // }
        // else
        // {
        //     InventorySaveData saveData = new InventorySaveData(inventorySystem, transform.position, transform.rotation);

        //     SaveGameManager.CurrentSaveData.chestDictionary.Add(uniqueID.ID, saveData);
        // }
    }

    public void Interact(Interactor interactor, out bool interactionSuccessful)
    {
        OnDynamicInventoryDisplayRequested?.Invoke(inventorySystem, displayName, 0);

        interactionSuccessful = true;
    }

    public void EndInteraction()
    {

    }
}