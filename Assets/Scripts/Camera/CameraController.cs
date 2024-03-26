using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private float onFootZoom;
    [SerializeField] private float uiZoom;
    [SerializeField] private float speed;

    private void Awake()
    {
        transform.position = TargetDestination;

        transform.LookAt(player);
    }

    private float ZoomDist => InputProvider.activeActionMap == InputProvider.ActionMaps.OnFoot ? onFootZoom : uiZoom;
    private Vector3 TargetDestination => player.position + Vector3.up * ZoomDist + Vector3.back * ZoomDist;

    private void Update()
    {
        transform.position = Vector3.Lerp(transform.position, TargetDestination, Time.deltaTime * speed);
    }
}
