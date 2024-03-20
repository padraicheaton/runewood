using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public static class SpellComponentData
{
    public enum Element
    {
        Null,
        Fire,
        Water,
        Earth,
        Air
    }

    public enum Action
    {
        Missile,
        Explosion,
        Trap,
        Buff
    }

    public static SpellItemData CreateCustomSpell(Element element, List<Action> actions)
    {
        SpellItemData craftedSpell = ScriptableObject.CreateInstance<SpellItemData>();

        craftedSpell.element = element;
        craftedSpell.actions = actions;

        // Can safely add this to the database as it will assign an available ID to it
        ItemManager.GetDatabase().AddCustomCraftedSpell(craftedSpell);

        SaveGameManager.CurrentSaveData.craftedSpellData.AddCraftedSpell(craftedSpell);

        return craftedSpell;
    }
}

[System.Serializable]
public class CraftedSpellData
{
    [System.Serializable]
    public struct SpellSaveData
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

    public List<InventoryItemData> GetAllSpellItems()
    {
        List<InventoryItemData> allSpellItems = new List<InventoryItemData>();

        foreach (SpellSaveData spellSaveData in craftedSpells)
        {
            SpellItemData craftedSpell = ScriptableObject.CreateInstance<SpellItemData>();

            craftedSpell.ID = spellSaveData.ID;
            craftedSpell.element = spellSaveData.element;
            craftedSpell.actions = spellSaveData.actions;

            allSpellItems.Add(craftedSpell);
        }

        return allSpellItems;
    }
}