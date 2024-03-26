using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlaceableItem : MonoBehaviour
{
    protected PlaceableItemData itemData;

    public virtual void Initialise(PlaceableItemData itemData)
    {
        // Add self to area manager
        this.itemData = itemData;

        AreaManager.Instance.RecordPlacedItem(this.itemData, transform);
    }

    protected void OnObjectDestroyed() // To be called when destroyed
    {
        AreaManager.Instance.TryRemovePlacedItem(itemData, transform);
    }
}
