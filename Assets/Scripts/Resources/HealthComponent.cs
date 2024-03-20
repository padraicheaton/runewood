using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthComponent : ResourceComponent
{
    public enum DeathBehaviour
    {
        Custom,
        Destroy
    }

    [Header("Health Settings")]
    [SerializeField] private DeathBehaviour deathBehaviour;

    private void Start()
    {
        if (deathBehaviour == DeathBehaviour.Destroy)
            onResourceDepleted += () => Destroy(gameObject);
    }
}
