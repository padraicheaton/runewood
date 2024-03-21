using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomSpellGenerator : ItemGenerator
{
    public override void Interact(Interactor interactor, out bool interactionSuccessful)
    {
        var allElements = Enum.GetValues(typeof(SpellComponentData.Element));
        var allActions = Enum.GetValues(typeof(SpellComponentData.Action));

        SpellComponentData.Element element = (SpellComponentData.Element)UnityEngine.Random.Range(0, allElements.Length);

        List<SpellComponentData.Action> actions = new List<SpellComponentData.Action>();

        for (int i = 0; i < UnityEngine.Random.Range(1, 4); i++)
        {
            actions.Add((SpellComponentData.Action)UnityEngine.Random.Range(0, allActions.Length));
        }

        SpellItemData spellItemData = SpellComponentData.CreateCustomSpell(element, actions);

        GenerateItem(spellItemData);

        interactionSuccessful = true;
    }
}
