using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BillboardUI : MonoBehaviour
{
    private Transform cameraTransform;

    private void Start()
    {
        cameraTransform = CameraController.Instance.transform;
    }

    private void Update()
    {
        transform.LookAt(cameraTransform);
        transform.Rotate(0, 180, 0);
    }
}
