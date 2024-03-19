using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class DynamicInventoryDisplay : InventoryDisplay
{
    [SerializeField] protected InventorySlot_UI slotPrefab;
    [SerializeField] protected Transform slotParent;
    [SerializeField] protected TextMeshProUGUI titleTxt;

    //! Could look into object pooling to make this process more efficient, instead of constantly deleting and re-instanitating

    protected override void Start()
    {
        base.Start();
    }

    public void RefreshDynamicInventory(InventorySystem invToDisplay, string title, int offset)
    {
        ClearSlots();

        inventorySystem = invToDisplay;

        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChanged += UpdateSlot;

        AssignSlots(inventorySystem, offset);

        titleTxt.text = title;
    }

    public override void AssignSlots(InventorySystem invToDisplay, int offset)
    {
        slotDictionary = new Dictionary<InventorySlot_UI, InventorySlot>();

        if (invToDisplay == null)
            return;

        for (int i = offset; i < invToDisplay.InventorySize; i++)
        {
            InventorySlot_UI uiSlot = Instantiate(slotPrefab, slotParent);

            slotDictionary.Add(uiSlot, invToDisplay.InventorySlots[i]);

            uiSlot.Init(invToDisplay.InventorySlots[i]);
            uiSlot.UpdateUISlot();
        }
    }

    private void ClearSlots()
    {
        foreach (Transform child in slotParent.Cast<Transform>())
        {
            Destroy(child.gameObject);
        }

        if (slotDictionary != null)
            slotDictionary.Clear();

        titleTxt.text = "";
    }

    private void OnDisable()
    {
        if (inventorySystem != null)
            inventorySystem.OnInventorySlotChanged -= UpdateSlot;
    }
}
