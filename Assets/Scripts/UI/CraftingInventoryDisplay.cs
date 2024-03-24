using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingInventoryDisplay : StaticInventoryDisplay
{
    [Header("References")]
    [SerializeField] private Button combineBtn;

    protected override void Start()
    {
        base.Start();

        combineBtn.interactable = false;
        combineBtn.onClick.AddListener(OnCombineBtnPressed);

        foreach (InventorySlot_UI slot in slots)
            slot.OnItemDataChanged += OnCraftingItemSlotChanged;
    }

    private void OnCraftingItemSlotChanged()
    {
        // Check for a valid recipe, only enable slots if the prerequisite is valid

        // Element rune slot is at index 0
        bool elementRuneInSlot = slots[0].AssignedInventorySlot.Data != null && slots[0].AssignedInventorySlot.Data.GetType() == typeof(ElementItemData);

        List<SpellComponentData.Action> checkedActions = new List<SpellComponentData.Action>();
        int totalActions = 0;

        for (int i = 1; i < slots.Length; i++)
        {
            InventorySlot_UI inventorySlotUI = slots[i];

            if (inventorySlotUI.AssignedInventorySlot.Data == null)
                break;

            totalActions++;

            RuneItemData runeItemData = inventorySlotUI.AssignedInventorySlot.Data as RuneItemData;

            if (checkedActions.Count == 0)
            {
                checkedActions.Add(runeItemData.action);
                continue;
            }
            else
            {
                // Only add action here if the previous one can chain
                if (SpellComponentData.CanChainAfterAction(checkedActions.Last()))
                {
                    checkedActions.Add(runeItemData.action);
                }
            }
        }

        // Will add more checks in here later
        bool isValidRecipe = elementRuneInSlot && totalActions >= 1 && totalActions == checkedActions.Count;

        combineBtn.interactable = isValidRecipe;
    }

    public void OnCombineBtnPressed()
    {
        // Actually craft things together
        SpellComponentData.Element element = (slots[0].AssignedInventorySlot.Data as ElementItemData).element;

        List<SpellComponentData.Action> actions = new List<SpellComponentData.Action>();

        for (int i = 1; i < slots.Length; i++)
        {
            if (slots[i].AssignedInventorySlot.Data == null)
                break;

            actions.Add((slots[i].AssignedInventorySlot.Data as RuneItemData).action);
        }

        SpellItemData craftedSpell = SpellComponentData.CreateCustomSpell(element, actions);

        PlayerInventoryHolder.OnAddToPlayerInventoryRequested?.Invoke(craftedSpell, 1);

        // Remove one item from each slot that contains one
        foreach (InventorySlot_UI slot in slots)
        {
            if (slot.AssignedInventorySlot.Data == null)
                continue;

            if (slot.AssignedInventorySlot.StackSize == 1)
                slot.ClearSlot();
            else
            {
                slot.AssignedInventorySlot.RemoveFromStack(1);
                slot.UpdateUISlot();
            }
        }
    }

    public void ReturnAnyItemsToPlayer()
    {
        foreach (InventorySlot_UI slot in slots)
        {
            if (slot.AssignedInventorySlot.Data == null)
                continue;

            PlayerInventoryHolder.OnAddToPlayerInventoryRequested(slot.AssignedInventorySlot.Data, slot.AssignedInventorySlot.StackSize);

            slot.ClearSlot();
        }
    }
}
