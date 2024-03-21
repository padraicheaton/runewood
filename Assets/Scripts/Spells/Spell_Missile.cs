using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spell_Missile : Spell
{
    [Header("Settings")]
    [SerializeField] private float acceleration;

    private Rigidbody rb;
    private float timer;
    private float activationTime = 0.25f;

    protected override void OnSetup()
    {
        timer = 0f;

        rb = GetComponent<Rigidbody>();

        rb.velocity = transform.forward * acceleration;
    }

    private void FixedUpdate()
    {
        rb.AddForce(transform.forward * acceleration, ForceMode.Acceleration);

        timer += Time.fixedDeltaTime;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (timer >= activationTime && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Detonate();
    }

    protected override void FireDetonate()
    {

    }

    protected override void WaterDetonate()
    {

    }

    protected override void EarthDetonate()
    {

    }

    protected override void AirDetonate()
    {

    }

    protected override void OnDetonationFinish()
    {
        SpellCaster.OnSpellCastRequested?.Invoke(transform.position, -transform.forward, element, actions);

        Destroy(gameObject);
    }
}
