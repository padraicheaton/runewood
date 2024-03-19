using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{
    public static CameraController Instance;

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float onFootZoom;
    [SerializeField] private float uiZoom;
    [SerializeField] private float speed;
    [SerializeField] private float deadZoneThreshold;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(this);
    }

    private float ZoomDist => InputProvider.activeActionMap == InputProvider.ActionMaps.OnFoot ? onFootZoom : uiZoom;
    private Vector3 TargetDestination => player.position + Vector3.up * ZoomDist + Vector3.back * ZoomDist;

    private void Start()
    {
        transform.position = TargetDestination;

        transform.LookAt(player);
    }

    private void Update()
    {
        if (Vector3.Distance(transform.position, TargetDestination) >= deadZoneThreshold)
            transform.position = Vector3.Lerp(transform.position, TargetDestination, Time.deltaTime * speed);
    }
}
