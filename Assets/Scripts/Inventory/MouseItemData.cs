using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.InputSystem;

public class MouseItemData : MonoBehaviour
{

    public static MouseItemData Instance;

    public Image itemSprite;
    public TextMeshProUGUI itemCountTxt;
    public InventorySlot InventorySlot;

    public GameObject tooltipContainer;
    public TextMeshProUGUI tooltipNameTxt, tooltipDescriptionTxt;

    private InventorySlot_UI hoveredItemSlot;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        itemSprite.color = Color.clear;
        itemCountTxt.text = "";

        tooltipContainer.SetActive(false);
    }

    public void UpdateMouseSlot(InventorySlot slot)
    {
        InventorySlot.AssignItem(slot);

        itemSprite.sprite = slot.Data.icon;
        itemCountTxt.text = slot.StackSize > 1 ? slot.StackSize.ToString() : "";
        itemSprite.color = Color.white;
    }

    public void UpdateMouseSlot()
    {
        itemSprite.sprite = InventorySlot.Data.icon;
        itemCountTxt.text = InventorySlot.StackSize > 1 ? InventorySlot.StackSize.ToString() : "";
        itemSprite.color = Color.white;
    }

    public void ClearSlot()
    {
        InventorySlot.ClearSlot();

        itemSprite.color = Color.clear;
        itemCountTxt.text = "";
    }

    private void Update()
    {
        if (InputProvider.activeActionMap == InputProvider.ActionMaps.UI)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }

    public void HoveredOverItemSlot(InventorySlot_UI slot)
    {
        hoveredItemSlot = slot;

        if (hoveredItemSlot.AssignedInventorySlot.Data != null)
        {
            tooltipContainer.SetActive(true);
            tooltipNameTxt.text = hoveredItemSlot.AssignedInventorySlot.Data.GetDisplayName();
            tooltipDescriptionTxt.text = hoveredItemSlot.AssignedInventorySlot.Data.GetDescription();
        }
        else
        {
            tooltipContainer.SetActive(false);
        }
    }

    public void ExitHoveredItemSlot(InventorySlot_UI slot)
    {
        if (hoveredItemSlot == slot)
        {
            // If exiting the one that has just been hovered over, i.e. not switching to then hover over another slot
            tooltipContainer.SetActive(false);
        }
    }
}
