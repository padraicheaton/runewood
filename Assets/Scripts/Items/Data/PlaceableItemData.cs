using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Placeable Item")]
public class PlaceableItemData : UseableItemData
{
    [Header("Placeable Item")]
    public GameObject prefab;

    protected override void OnItemUsed()
    {
        throw new System.NotImplementedException();
    }
}
