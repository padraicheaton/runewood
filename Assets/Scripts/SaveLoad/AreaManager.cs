using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AreaManager : Singleton<AreaManager>
{
    private string AreaSaveKey => SceneController.Instance.ActiveScene.ToString();

    private void Awake()
    {
        SceneController.Instance.OnSceneLoaded += LoadPlaceableItemsForScene;
    }

    private void Start()
    {
        LoadPlaceableItemsForScene(SceneController.Instance.ActiveScene);
    }

    private void LoadPlaceableItemsForScene(SceneController.Scene scene)
    {
        List<PlaceableItemSaveData> itemsPlacedInThisScene = new List<PlaceableItemSaveData>();

        foreach (KeyValuePair<PlaceableItemSaveData, string> keyValuePair in SaveGameManager.CurrentSaveData.placedItemDictionary)
        {
            if (keyValuePair.Value == AreaSaveKey)
                itemsPlacedInThisScene.Add(keyValuePair.Key);
        }


        foreach (PlaceableItemSaveData saveData in itemsPlacedInThisScene)
        {
            GameObject prefab = (ItemManager.GetItem(saveData.itemId) as PlaceableItemData).prefab;

            if (prefab == null)
                continue;

            GameObject instance = Instantiate(prefab, saveData.position, saveData.rotation);

            // If it is a chest, try and load the inventory data
            if (instance.TryGetComponent<ChestData>(out ChestData chestDataComponent))
            {
                chestDataComponent.TryLoadInventory(saveData);
            }

            MoveTransformToActiveScene(instance.transform);
        }
    }

    public void RecordPlacedItem(PlaceableItemData itemData, Transform itemTransform)
    {
        PlaceableItemSaveData saveData = new PlaceableItemSaveData(itemData.ID, itemTransform.position, itemTransform.rotation);

        SaveGameManager.CurrentSaveData.placedItemDictionary.Add(saveData, AreaSaveKey);

        MoveTransformToActiveScene(itemTransform);
    }

    public void RecordPlacedChest(PlaceableItemData itemData, InventorySystem invSystem, Transform itemTransform)
    {
        PlaceableItemSaveData saveData = new PlaceableItemSaveData(itemData.ID, itemTransform.position, itemTransform.rotation, invSystem);

        SaveGameManager.CurrentSaveData.placedItemDictionary.Add(saveData, AreaSaveKey);

        MoveTransformToActiveScene(itemTransform);
    }

    public void TryRemovePlacedItem(PlaceableItemData itemData, Transform itemTransform)
    {
        foreach (KeyValuePair<PlaceableItemSaveData, string> keyValuePair in SaveGameManager.CurrentSaveData.placedItemDictionary)
        {
            if (keyValuePair.Key.itemId == itemData.ID && keyValuePair.Key.position == itemTransform.position && keyValuePair.Key.rotation == itemTransform.rotation)
            {
                SaveGameManager.CurrentSaveData.placedItemDictionary.Remove(keyValuePair.Key);
                break;
            }
        }
    }

    private void MoveTransformToActiveScene(Transform itemTransform)
    {
        SceneController.Instance.MoveGameObjectToActiveScene(itemTransform.gameObject);
    }
}

[System.Serializable]
public class PlaceableItemSaveData
{
    public int itemId; // Refers to a PlaceableItemData in the Item Database -> this holds the prefab
    public Vector3 position;
    public Quaternion rotation;
    public InventorySystem inventorySystem;

    public PlaceableItemSaveData(int _itemId, Vector3 _position, Quaternion _rotation)
    {
        itemId = _itemId;
        position = _position;
        rotation = _rotation;
        inventorySystem = null;
    }

    public PlaceableItemSaveData(int _itemId, Vector3 _position, Quaternion _rotation, InventorySystem _invSystem)
    {
        itemId = _itemId;
        position = _position;
        rotation = _rotation;
        inventorySystem = _invSystem;
    }
}
