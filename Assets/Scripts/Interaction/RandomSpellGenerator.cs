using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpellGenerator : ItemGenerator
{
    public override void Interact(Interactor interactor, out bool interactionSuccessful)
    {
        SpellItemData spellItemData = SpellComponentData.CreateCustomSpell(SpellComponentData.Element.Water, new List<SpellComponentData.Action>() { SpellComponentData.Action.Explosion });

        GenerateItem(spellItemData);

        interactionSuccessful = true;
    }
}
