using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class PlayerCombat : MonoBehaviour
{
    public enum PlayerResources
    {
        Health,
        Mana
    }

    [Header("References")]
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private ResourceComponent healthComponent;
    [SerializeField] private ResourceComponent manaComponent;

    [Header("Visual Settings")]
    [SerializeField] private float damageTakenKnockAmount;

    public static UnityAction<ConsumableItemData> onConsumableItemUsed;
    public static UnityAction<SpellItemData> onSpellItemUsed;

    private void Start()
    {
        onConsumableItemUsed += OnConsumableItemUsed;
        onSpellItemUsed += OnSpellItemUsed;

        healthComponent.onResourceTakenFrom += OnDamageTaken;
    }

    private void OnDestroy()
    {
        onConsumableItemUsed -= OnConsumableItemUsed;
        onSpellItemUsed -= OnSpellItemUsed;
    }

    private void OnSpellItemUsed(SpellItemData spell)
    {
        Debug.Log($"Should cast {spell.element} {spell.action}");
    }

    private void OnConsumableItemUsed(ConsumableItemData consumableItemData)
    {
        ResourceChangingItemData resourceChangingItemData = consumableItemData as ResourceChangingItemData;

        if (resourceChangingItemData != null)
        {
            OnResourceChangingItemused(resourceChangingItemData);

            return;
        }

        // Else do something else...
    }

    private void OnResourceChangingItemused(ResourceChangingItemData resourceChangingItemData)
    {
        switch (resourceChangingItemData.resource)
        {
            case PlayerResources.Health:
            default:
                healthComponent.ReplenishResource(resourceChangingItemData.amount);
                break;
            case PlayerResources.Mana:
                manaComponent.ReplenishResource(resourceChangingItemData.amount);
                break;
        }
    }

    private void OnDamageTaken(float percentage)
    {
        playerMovement.ApplyImpulseForce((Vector3.up - transform.forward) * damageTakenKnockAmount);
    }
}
