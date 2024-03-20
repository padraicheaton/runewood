using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Spell Item")]
public class SpellItemData : UseableItemData
{
    [Header("Spell Settings")]
    public SpellComponentData.Element element;
    public List<SpellComponentData.Action> actions;

    protected override void OnItemUsed()
    {
        PlayerCombat.onSpellItemUsed?.Invoke(this);
    }
}
