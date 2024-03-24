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

    private void OnEnable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged += RefreshStaticDisplay;
    }

    private void OnDisable()
    {
        PlayerInventoryHolder.OnPlayerInventoryChanged -= RefreshStaticDisplay;
    }

    protected override void Start()
    {
        base.Start();

        activeSlotIndex = 0;
        maximumSlotIndex = slots.Length - 1;

        ActiveSlotUI.SetHighlighted(true);

        InputProvider.onUseItemButtonPressed += TryUseItem;
    }

    private void ChangeActiveSlot(int index)
    {
        ActiveSlotUI.SetHighlighted(false);

        activeSlotIndex = index;

        if (activeSlotIndex > maximumSlotIndex)
            activeSlotIndex = 0;
        else if (activeSlotIndex < 0)
            activeSlotIndex = maximumSlotIndex;

        ActiveSlotUI.SetHighlighted(true);
    }

    private void TryUseItem(int index)
    {
        ChangeActiveSlot(index);

        if (ActiveItem == null)
            return;

        UseableItemData useableItemData = ActiveItem as UseableItemData;

        if (useableItemData == null)
            return;

        useableItemData.TryUseItem(ActiveSlotUI.AssignedInventorySlot);

        ActiveSlotUI.UpdateUISlot();
    }
}
