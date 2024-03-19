using System.Collections;
using System.Collections.Generic;
using System.Data.Common;
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

        SaveLoad.OnLoadGame += LoadInventory;
    }

    private void Start()
    {
        InventorySaveData chestSaveData = new InventorySaveData(inventorySystem, transform.position, transform.rotation);

        SaveGameManager.CurrentSaveData.chestDictionary.Add(uniqueID.ID, chestSaveData);
    }

    protected override void LoadInventory(SaveData data)
    {
        if (data.chestDictionary.TryGetValue(uniqueID.ID, out InventorySaveData chestSaveData))
        {
            inventorySystem = chestSaveData.invSystem;
            transform.position = chestSaveData.position;
            transform.rotation = chestSaveData.rotation;
        }
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