using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class CraftingInventoryDisplay : StaticInventoryDisplay
{
    [Header("References")]
    [SerializeField] private Button combineBtn;

    private List<CraftingRecipe> recipes;

    protected override void Start()
    {
        base.Start();

        recipes = Resources.LoadAll<CraftingRecipe>("Recipes").ToList();

        combineBtn.interactable = false;
        combineBtn.onClick.AddListener(OnCombineBtnPressed);

        foreach (InventorySlot_UI slot in slots)
            slot.OnItemDataChanged += OnCraftingItemSlotChanged;
    }

    private void OnCraftingItemSlotChanged()
    {
        bool isValidRecipe = IsValidSpellRecipe() || IsValidItemCraftingRecipe();

        combineBtn.interactable = isValidRecipe;
    }

    public void OnCombineBtnPressed()
    {
        InventoryItemData craftedItem = null;

        if (IsValidSpellRecipe())
        {
            craftedItem = CraftSpell();
        }
        else if (IsValidItemCraftingRecipe())
        {
            craftedItem = TryGetCraftingRecipe().result;
        }

        PlayerInventoryHolder.OnAddToPlayerInventoryRequested?.Invoke(craftedItem, 1);

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

    private InventoryItemData CraftSpell()
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

        return craftedSpell;
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

    private bool IsValidSpellRecipe()
    {
        // Element rune slot is at index 0
        bool elementRuneInSlot = slots[0].AssignedInventorySlot.Data != null && slots[0].AssignedInventorySlot.Data.GetType() == typeof(ElementItemData);

        if (!elementRuneInSlot)
            return false;

        List<SpellComponentData.Action> checkedActions = new List<SpellComponentData.Action>();
        int totalActions = 0;

        for (int i = 1; i < slots.Length; i++)
        {
            InventorySlot_UI inventorySlotUI = slots[i];

            if (inventorySlotUI.AssignedInventorySlot.Data == null)
                break;

            totalActions++;

            RuneItemData runeItemData = inventorySlotUI.AssignedInventorySlot.Data as RuneItemData;

            if (runeItemData == null) // if there is an element rune in the middle but not on the outside
                return false;

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
        bool isValidRecipe = elementRuneInSlot && totalActions >= 1 && totalActions == checkedActions.Count && SpellComponentData.CanFinishOnAction(checkedActions.Last());

        return isValidRecipe;
    }

    private bool IsValidItemCraftingRecipe()
    {
        return TryGetCraftingRecipe() != null;
    }

    private CraftingRecipe TryGetCraftingRecipe()
    {
        List<InventoryItemData> ingredients = new List<InventoryItemData>();

        foreach (InventorySlot_UI slot in slots)
        {
            if (slot.AssignedInventorySlot.Data != null)
                ingredients.Add(slot.AssignedInventorySlot.Data);
        }

        if (ingredients.Count == 0)
            return null;

        foreach (CraftingRecipe recipe in recipes)
        {
            if (recipe.ingredients.SequenceEqual(ingredients))
                return recipe;
        }

        return null;
    }
}
