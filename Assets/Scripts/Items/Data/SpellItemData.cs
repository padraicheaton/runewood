using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpellItemData : UseableItemData
{
    [Header("Spell Settings")]
    public SpellComponentData.Element element;
    public List<SpellComponentData.Action> actions;

    protected override void OnItemUsed()
    {
        PlayerCombat.onSpellItemUsed?.Invoke(this);
    }

    public override string GetDisplayName()
    {
        return $"{element} Spell";
    }

    public override string GetDescription()
    {
        return string.Join("\n", actions);
    }
}
