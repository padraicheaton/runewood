using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Placeable Item")]
public class PlaceableItemData : ConsumableItemData
{
    [Header("Placeable Item")]
    public GameObject prefab;

    protected override void OnItemUsed()
    {
        PlayerBuildingController.OnPlaceableItemUsed?.Invoke(this);
    }
}
