using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BobCanvas : MonoBehaviour
{
    [Header("Settings")]
    [SerializeField] private float bobHeight;
    [SerializeField] private float bobSpeed;

    private float startY;
    private float randomOffset;

    private void Start()
    {
        startY = transform.position.y;
        randomOffset = Random.Range(0f, 10f);
    }

    private void Update()
    {
        Vector3 newPos = transform.position;

        newPos.y = startY + Mathf.Sin(Time.time) * bobHeight;

        //transform.position = transform.position + Vector3.up * startY * Mathf.Sin((Time.time * bobSpeed) + randomOffset) * bobHeight;
        transform.position = newPos;
    }
}
