using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ResourceDisplay : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ResourceComponent resourceComponent;
    [SerializeField] private Image fillImg;

    private void Start()
    {
        resourceComponent.onResourceAmountChanged += UpdateDisplay;

        UpdateDisplay(resourceComponent.ResourcePercentage);
    }

    private void OnDestroy()
    {
        resourceComponent.onResourceAmountChanged -= UpdateDisplay;
    }

    private void UpdateDisplay(float percentage)
    {
        fillImg.fillAmount = percentage;
    }
}
