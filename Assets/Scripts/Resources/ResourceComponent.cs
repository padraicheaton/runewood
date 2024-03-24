using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class ResourceComponent : MonoBehaviour
{
    [Header("Resource Settings")]
    [SerializeField] private float maximumAmount;
    [SerializeField] private bool doesRegenerateNaturally;
    [SerializeField] private float regenerationDelay;
    [SerializeField] private float regenerationTickAmount;

    private float timer;

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

        SaveGameManager.OnGameSuccessfullyLoaded += (saveData) => ReplenishResource(maximumAmount);
    }

    private void Update()
    {
        if (doesRegenerateNaturally && ResourcePercentage < 1f)
        {
            if (timer < regenerationDelay)
                timer += Time.deltaTime;
            else
            {
                ReplenishResource(regenerationTickAmount);
                timer = 0f;
            }
        }
    }

    private void OnDestroy()
    {
        onResourceReplenished -= (percentage) => onResourceAmountChanged?.Invoke(percentage);
        onResourceTakenFrom -= (percentage) => onResourceAmountChanged?.Invoke(percentage);

        onResourceAmountChanged -= (percentage) => CheckForResourcedDepleted();
    }

    public void ReplenishResource(float amount)
    {
        if (amount < 0)
        {
            TakeFromResource(Mathf.Abs(amount));
            return;
        }

        currentAmount += amount;

        if (currentAmount > maximumAmount) currentAmount = maximumAmount;

        onResourceReplenished?.Invoke(ResourcePercentage);
    }

    public void TakeFromResource(float amount)
    {
        currentAmount -= amount;

        timer = 0f;

        onResourceTakenFrom?.Invoke(ResourcePercentage);
    }

    public bool HasAmount(float amount) => currentAmount >= amount;

    private void CheckForResourcedDepleted()
    {
        if (currentAmount < 0)
        {
            currentAmount = 0;

            onResourceDepleted?.Invoke();
        }
    }
}
