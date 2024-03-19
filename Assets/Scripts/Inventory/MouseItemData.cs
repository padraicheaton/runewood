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

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);

        itemSprite.color = Color.clear;
        itemCountTxt.text = "";
    }

    public void UpdateMouseSlot(InventorySlot slot)
    {
        InventorySlot.AssignItem(slot);

        itemSprite.sprite = slot.Data.icon;
        itemCountTxt.text = slot.StackSize > 1 ? slot.StackSize.ToString() : "";
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
        if (InventorySlot.Data != null)
        {
            transform.position = Mouse.current.position.ReadValue();
        }
    }
}
