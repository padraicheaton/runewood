using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;

public static class SpellComponentData
{
    public enum Element
    {
        Fire,
        Water,
        Earth,
        Air
    }

    public enum Action
    {
        Missile,
        Explosion,
        Split,
    }

    public static bool CanChainAfterAction(Action action)
    {
        switch (action)
        {
            default:
                return false;
            case Action.Missile:
            case Action.Split:
                return true;
        }
    }

    public static bool CanFinishOnAction(Action action)
    {
        switch (action)
        {
            default:
                return true;
            case Action.Split:
                return false;
        }
    }

    public static float GetActionManaCost(Action action)
    {
        switch (action)
        {
            default:
            case Action.Missile:
                return 10f;
            case Action.Explosion:
                return 15f;
            case Action.Split:
                return 15f;
        }
    }

    public static SpellItemData CreateCustomSpell(Element element, List<Action> actions)
    {
        if (SaveGameManager.CurrentSaveData.craftedSpellData.TryGetExistingSpell(element, actions, out SpellItemData existingSpell))
        {
            // If the spell I'm trying to create already exists, return it
            return existingSpell;
        }
        else
        {
            // SpellItemData craftedSpell = ScriptableObject.CreateInstance<SpellItemData>();

            // craftedSpell.element = element;
            // craftedSpell.actions = actions;

            CraftedSpellData.SpellSaveData craftedSpellData = new CraftedSpellData.SpellSaveData(-1, element, actions);

            SpellItemData craftedSpell = SaveGameManager.CurrentSaveData.craftedSpellData.CreateSpellItemFromSaveData(craftedSpellData);

            // Can safely add this to the database as it will assign an available ID to it
            ItemManager.GetDatabase().AddCustomCraftedSpell(craftedSpell);

            SaveGameManager.CurrentSaveData.craftedSpellData.AddCraftedSpell(craftedSpell);

            return craftedSpell;
        }
    }
}

[System.Serializable]
public class CraftedSpellData
{
    [System.Serializable]
    public class SpellSaveData
    {
        public int ID;
        public SpellComponentData.Element element;
        public List<SpellComponentData.Action> actions;

        public SpellSaveData(int _ID, SpellComponentData.Element _element, List<SpellComponentData.Action> _actions)
        {
            ID = _ID;
            element = _element;
            actions = _actions;
        }
    }

    public List<SpellSaveData> craftedSpells = new List<SpellSaveData>();

    public void AddCraftedSpell(SpellItemData spellItemData)
    {
        craftedSpells.Add(new SpellSaveData(spellItemData.ID, spellItemData.element, spellItemData.actions));
    }

    public bool TryGetExistingSpell(SpellComponentData.Element element, List<SpellComponentData.Action> actions, out SpellItemData existingSpell)
    {
        existingSpell = null;

        SpellSaveData foundSpell = craftedSpells.Find(spell => spell.element == element && spell.actions.SequenceEqual(actions));

        if (foundSpell != null)
        {
            existingSpell = CreateSpellItemFromSaveData(foundSpell);

            return true;
        }

        return false;
    }

    public List<InventoryItemData> GetAllSpellItems()
    {
        List<InventoryItemData> allSpellItems = new List<InventoryItemData>();

        foreach (SpellSaveData spellSaveData in craftedSpells)
        {
            SpellItemData craftedSpell = CreateSpellItemFromSaveData(spellSaveData);

            allSpellItems.Add(craftedSpell);
        }

        return allSpellItems;
    }

    public SpellItemData CreateSpellItemFromSaveData(SpellSaveData spellSaveData)
    {
        SpellItemData spell = ScriptableObject.CreateInstance<SpellItemData>();

        spell.ID = spellSaveData.ID;
        spell.element = spellSaveData.element;
        spell.actions = spellSaveData.actions;

        spell.icon = Resources.Load<Sprite>("Spell");

        return spell;
    }
}