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
    [SerializeField] private List<SpellComponentData.Element> elementsWeakTo;
    [SerializeField] private List<SpellComponentData.Element> elementsStrongAgainst;

    private void Start()
    {
        if (deathBehaviour == DeathBehaviour.Destroy)
            onResourceDepleted += () => Destroy(gameObject);
    }

    public void TakeDamage(SpellComponentData.Element element, float amount)
    {
        if (elementsWeakTo.Contains(element))
            amount *= 2f;
        else if (elementsStrongAgainst.Contains(element))
            amount *= 0.5f;

        Debug.Log($"Taking {amount} damage");

        TakeFromResource(amount);
    }
}
