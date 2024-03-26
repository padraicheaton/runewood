using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerBuildingController : MonoBehaviour
{
    public static UnityAction<PlaceableItemData> OnPlaceableItemUsed;

    private void Awake()
    {
        OnPlaceableItemUsed += TryPlaceItem;
    }

    private void TryPlaceItem(PlaceableItemData item)
    {
        //! DEBUG

        GameObject instance = Instantiate(item.prefab, transform.position + transform.forward, Quaternion.identity);

        if (instance.TryGetComponent<PlaceableItem>(out PlaceableItem placeableItemComponent))
        {
            placeableItemComponent.Initialise(item);
        }
        else
            Debug.LogWarning("Placed item without a PlaceableItem component!");
    }
}
