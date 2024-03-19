using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Runewood/Inventory/Spell Item")]
public class SpellItemData : UseableItemData
{
    [Header("Spell Settings")]
    public SpellComponentData.Element element;
    public SpellComponentData.Action action;

    protected override void OnItemUsed()
    {
        Debug.Log($"I cast {element} {action}!!");
    }
}
