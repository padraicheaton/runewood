using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spell_Explosion : Spell
{
    [Header("Settings")]
    [SerializeField] private float radius;

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
        Destroy(gameObject, 1f);
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position, radius);
    }
}
