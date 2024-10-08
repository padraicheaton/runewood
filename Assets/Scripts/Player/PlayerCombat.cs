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
    [SerializeField] private Transform castOrigin;

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
        float totalManaCost = 0f;

        foreach (SpellComponentData.Action action in spell.actions)
            totalManaCost += SpellComponentData.GetActionManaCost(action);

        if (manaComponent.HasAmount(totalManaCost))
        {
            SpellCaster.OnItemSpellCastRequested?.Invoke(GetCastOrigin(), GetCastForward(), spell);

            manaComponent.TakeFromResource(totalManaCost);
        }
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

    private Vector3 GetCastOrigin()
    {
        if (CameraController.Instance.ActiveViewMode == CameraController.ViewMode.TopDown)
            return castOrigin.position;
        else
        {
            return transform.position + Vector3.up + CameraController.Instance.transform.forward;
        }
    }

    private Vector3 GetCastForward()
    {
        if (CameraController.Instance.ActiveViewMode == CameraController.ViewMode.TopDown)
            return castOrigin.forward;
        else
        {
            return CameraController.Instance.transform.forward;
        }
    }

    private void OnDamageTaken(float percentage)
    {
        if (playerMovement.IsGrounded())
            playerMovement.ApplyImpulseForce((Vector3.up - transform.forward * 0.5f) * damageTakenKnockAmount, true);
    }
}
