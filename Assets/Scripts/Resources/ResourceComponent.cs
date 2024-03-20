using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceComponent : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private float maximumAmount;

    protected float currentAmount;

    public float ResourcePercentage => currentAmount / maximumAmount;
    public UnityAction onResourceDepleted;
    public UnityAction<float> onResourceReplenished;
    public UnityAction<float> onResourceTakenFrom;
    public UnityAction<float> onResourceAmountChanged;

    private void Awake()
    {
        currentAmount = maximumAmount;

        onResourceReplenished += (percentage) => onResourceAmountChanged?.Invoke(percentage);
        onResourceTakenFrom += (percentage) => onResourceAmountChanged?.Invoke(percentage);

        onResourceAmountChanged += (percentage) => CheckForResourcedDepleted();
    }

    private void OnDestroy()
    {
        onResourceReplenished -= (percentage) => onResourceAmountChanged?.Invoke(percentage);
        onResourceTakenFrom -= (percentage) => onResourceAmountChanged?.Invoke(percentage);

        onResourceAmountChanged -= (percentage) => CheckForResourcedDepleted();
    }

    public void ReplenishResource(float amount)
    {
        currentAmount += amount;

        if (currentAmount > maximumAmount) currentAmount = maximumAmount;

        onResourceReplenished?.Invoke(ResourcePercentage);
    }

    public void TakeFromResource(float amount)
    {
        currentAmount -= amount;

        onResourceTakenFrom?.Invoke(ResourcePercentage);
    }

    private void CheckForResourcedDepleted()
    {
        if (currentAmount < 0)
        {
            currentAmount = 0;

            onResourceDepleted?.Invoke();
        }
    }
}
