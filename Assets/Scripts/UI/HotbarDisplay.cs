using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class HotbarDisplay : StaticInventoryDisplay
{
    private int activeSlotIndex;
    private int maximumSlotIndex;

    private InventorySlot_UI ActiveSlotUI => slots[activeSlotIndex];
    private InventoryItemData ActiveItem => ActiveSlotUI.AssignedInventorySlot.Data;

    protected override void Start()
    {
        base.Start();

        activeSlotIndex = 0;
        maximumSlotIndex = slots.Length - 1;

        ActiveSlotUI.SetHighlighted(true);

        InputProvider.onUseItemButtonPressed += TryUseItem;
    }

    private void Update()
    {
        if (InputProvider.ChangeActiveSlotInput != 0)
            ChangeActiveSlot(Mathf.RoundToInt(Mathf.Sign(InputProvider.ChangeActiveSlotInput)));
    }

    private void ChangeActiveSlot(int direction)
    {
        ActiveSlotUI.SetHighlighted(false);

        activeSlotIndex += direction;

        if (activeSlotIndex > maximumSlotIndex)
            activeSlotIndex = 0;
        else if (activeSlotIndex < 0)
            activeSlotIndex = maximumSlotIndex;

        ActiveSlotUI.SetHighlighted(true);
    }

    private void TryUseItem()
    {
        if (ActiveItem == null)
            return;

        UseableItemData useableItemData = ActiveItem as UseableItemData;

        if (useableItemData == null)
            return;

        useableItemData.TryUseItem(ActiveSlotUI.AssignedInventorySlot);

        ActiveSlotUI.UpdateUISlot();
    }
}
