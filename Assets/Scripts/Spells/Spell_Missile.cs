using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Spell_Missile : MonoBehaviour, ISpell
{
    [Header("Settings")]
    [SerializeField] private float acceleration;

    private Rigidbody rb;
    private float timer;
    private float activationTime = 0.25f;

    private SpellComponentData.Element element;
    private List<SpellComponentData.Action> actions;

    public void Setup(SpellComponentData.Element element, List<SpellComponentData.Action> actions)
    {
        this.element = element;
        this.actions = actions;

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
        // Change this
        if (timer >= activationTime && other.gameObject.layer == LayerMask.NameToLayer("Ground"))
            Detonate();
    }

    public void Detonate()
    {
        Debug.Log("Chaine");
        SpellCaster.OnSpellCastRequested?.Invoke(transform.position, -transform.forward, element, actions);

        Destroy(gameObject);
    }
}
