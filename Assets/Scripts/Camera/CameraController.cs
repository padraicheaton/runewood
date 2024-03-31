using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : Singleton<CameraController>
{
    public enum ViewMode
    {
        OverTheShoulder,
        TopDown,
        UI
    }
    private ViewMode currentViewMode;

    [Header("References")]
    [SerializeField] private Transform player;

    [Header("Settings")]
    [SerializeField] private ViewMode defaultViewMode = ViewMode.OverTheShoulder;
    [SerializeField] private float speed;

    [Header("Top Down Settings")]
    [SerializeField] private float topDownZoom;

    [Header("Over The Shoulder Settings")]
    [SerializeField] private float overTheShoulderZoom;
    [SerializeField] private float shoulderOffset;
    [SerializeField] private float verticalOffset;
    [SerializeField] private float horizontalSens;
    [SerializeField] private float verticalSens;
    [SerializeField] private float verticalRotationClamp;

    private void Awake()
    {
        currentViewMode = defaultViewMode;

        transform.position = GetTargetDestination();
    }

    private void Update()
    {
        currentViewMode = InputProvider.activeActionMap == InputProvider.ActionMaps.OnFoot ? defaultViewMode : ViewMode.UI;

        if (currentViewMode == ViewMode.OverTheShoulder)
            HandleInput();

        if (currentViewMode == ViewMode.TopDown || currentViewMode == ViewMode.UI)
        {
            transform.position = Vector3.Lerp(transform.position, GetTargetDestination(), Time.deltaTime * speed);
            transform.rotation = Quaternion.Lerp(transform.rotation, GetTargetRotation(), Time.deltaTime * speed);
        }
        else
        {
            transform.position = GetTargetDestination();
            transform.rotation = GetTargetRotation();
        }
    }

    private void HandleInput()
    {
        Vector2 lookInput = InputProvider.LookInput;

        transform.Rotate(-lookInput.y * verticalSens, lookInput.x * horizontalSens, 0);

        // Zero out any z rotation
        Vector3 noZRotationEuler = transform.eulerAngles;
        noZRotationEuler.z = 0f;

        // Clamp the x rotation
        noZRotationEuler.x = Mathf.Clamp(NormalizeAngle(noZRotationEuler.x), -verticalRotationClamp, verticalRotationClamp);

        transform.rotation = Quaternion.Euler(noZRotationEuler);
    }

    private float NormalizeAngle(float angle)
    {
        angle = (angle + 180) % 360;
        if (angle < 0)
        {
            angle += 360;
        }
        return angle - 180;
    }

    private Vector3 GetTargetDestination()
    {
        if (currentViewMode == ViewMode.TopDown)
            return player.position + Vector3.up * topDownZoom + Vector3.back * topDownZoom;
        else if (currentViewMode == ViewMode.OverTheShoulder)
            return GetOverTheShoulderTargetPosition();

        // UI mode
        else
            return player.position + player.forward * 2f + Vector3.up;
    }

    private Vector3 GetOverTheShoulderTargetPosition()
    {
        Vector3 origin = player.position + Vector3.up * verticalOffset;

        Vector3 pos = origin - transform.forward * overTheShoulderZoom;

        pos += transform.right * shoulderOffset;

        return pos;
    }

    private Quaternion GetTargetRotation()
    {
        if (currentViewMode == ViewMode.TopDown)
            return Quaternion.Euler(new Vector3(45, 0f, 0f));
        else if (currentViewMode == ViewMode.UI)
            return Quaternion.LookRotation((player.position + Vector3.up) - transform.position);

        // For OTS mode rotation governed so just return the existing rotation
        else
            return transform.rotation;
    }
}
