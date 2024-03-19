using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(SphereCollider))]
public class ItemPickup : MonoBehaviour
{
    [SerializeField] private float pickupRadius;
    [SerializeField] private InventoryItemData itemData;
    [SerializeField] private int amount;
    [SerializeField] private Image iconImg;

    private SphereCollider sphereCollider;

    private Transform target;
    private float attractSpeed = 10f;

    private void Awake()
    {
        sphereCollider = GetComponent<SphereCollider>();
        sphereCollider.isTrigger = true;
        sphereCollider.radius = pickupRadius;
    }

    public void Init(InventoryItemData data)
    {
        itemData = data;

        iconImg.sprite = itemData.icon;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (itemData == null)
            return;

        if (other.TryGetComponent<PlayerInventoryHolder>(out PlayerInventoryHolder holder) && target == null)
        {
            bool successfullyAdded = holder.AddToInventory(itemData, amount);

            if (successfullyAdded)
                target = other.transform;
        }
    }

    private void FixedUpdate()
    {
        if (target != null)
        {
            transform.position = Vector3.MoveTowards(transform.position, target.position, Time.fixedDeltaTime * attractSpeed);

            if (Vector3.Distance(transform.position, target.position) < 0.1f)
            {
                Destroy(gameObject);
            }
        }
    }
}
