using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactor : MonoBehaviour
{
    [SerializeField] private Transform interactionPoint;
    [SerializeField] private LayerMask interactionLayer;
    [SerializeField] private float interactionPointRadius;
    public bool IsInteracting { get; private set; }

    private void Start()
    {
        InputProvider.onInteractButtonPressed += TryInteract;
    }

    private void OnDestroy()
    {
        InputProvider.onInteractButtonPressed -= TryInteract;
    }

    private void TryInteract()
    {
        foreach (Collider coll in Physics.OverlapSphere(interactionPoint.position, interactionPointRadius, interactionLayer))
        {
            if (coll.TryGetComponent<IInteractable>(out IInteractable interactable))
            {
                interactable.Interact(this, out bool isSuccessful);

                if (isSuccessful)
                    break;
            }
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(interactionPoint.position, interactionPointRadius);
    }
}
