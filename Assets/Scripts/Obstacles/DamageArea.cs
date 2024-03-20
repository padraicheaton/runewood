using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(BoxCollider))]
public class DamageArea : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float damageDealt;

    private BoxCollider boxCollider;

    private void Start()
    {
        boxCollider = GetComponent<BoxCollider>();

        boxCollider.isTrigger = true;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.TryGetComponent<HealthComponent>(out HealthComponent healthComponent))
        {
            healthComponent.TakeFromResource(damageDealt);
        }
    }
}
