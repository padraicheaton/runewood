using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Split : Spell
{
    [Header("Settings")]
    [SerializeField] private float offsetAmount;

    protected override void OnSetup()
    {
        Detonate();
    }

    protected override void AirDetonate()
    {

    }

    protected override void EarthDetonate()
    {

    }

    protected override void FireDetonate()
    {

    }

    protected override void WaterDetonate()
    {

    }

    protected override void OnDetonationFinish()
    {
        SpellCaster.OnSpellCastRequested?.Invoke(transform.position, transform.forward + Vector3.left * offsetAmount, element, actions);
        SpellCaster.OnSpellCastRequested?.Invoke(transform.position, transform.forward + Vector3.right * offsetAmount, element, actions);

        Destroy(gameObject);
    }
}
