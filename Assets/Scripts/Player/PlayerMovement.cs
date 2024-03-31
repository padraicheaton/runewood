using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
    [SerializeField] private Transform visualsTransform;
    [SerializeField] private ParticleSystem sprintDustFX;
    [SerializeField] private ParticleSystem jumpDustFX;

    [Header("Physical Settings")]
    [SerializeField] private float walkingSpeed;
    [SerializeField] private float sprintSpeed;
    [SerializeField] private float airStrafeSpeed;
    [SerializeField] private float groundCheckOffset;
    [SerializeField] private float groundCheckRadius;
    [SerializeField] private LayerMask whatIsGround;
    [SerializeField] private float groundedDrag;
    [SerializeField] private float airDrag;
    [SerializeField] private float jumpForce;

    [Header("Visual Settings")]
    [SerializeField] private float turnSpeed;


    private Rigidbody motor;
    private Vector3 transformedInputDirection;

    private void OnEnable()
    {
        PlayerSaveData.onPlayerDataLoaded += LoadPlayerLocation;
    }

    private void OnDisable()
    {
        PlayerSaveData.onPlayerDataLoaded -= LoadPlayerLocation;
    }

    private void Start()
    {
        motor = GetComponent<Rigidbody>();

        InputProvider.onJumpButtonPressed += Jump;
    }

    private void Update()
    {
        // Calculate desired world direction from movement input
        transformedInputDirection = CalculateDesiredWorldDirection(InputProvider.MovementInput);

        if (transformedInputDirection.magnitude > 1f)
            transformedInputDirection.Normalize();

        HandleAnimations();

        motor.drag = IsGrounded() ? groundedDrag : airDrag;
    }

    private Vector3 CalculateDesiredWorldDirection(Vector2 input)
    {
        Transform cam = CameraController.Instance.transform;

        Vector3 direction = cam.right * input.x + cam.forward * input.y;
        direction.y = 0f;

        return direction.normalized;
    }

    private void FixedUpdate()
    {
        float speed = InputProvider.SprintPressed ? sprintSpeed : walkingSpeed;

        if (!IsGrounded())
            speed = airStrafeSpeed;

        if (motor.velocity.magnitude <= speed)
            motor.AddForce(transformedInputDirection * speed, ForceMode.Impulse);
    }

    private void HandleAnimations()
    {
        // Rotate towards input direction
        if (transformedInputDirection.magnitude > 0f)
            visualsTransform.forward = Vector3.Slerp(visualsTransform.forward, transformedInputDirection, Time.deltaTime * turnSpeed);

        anim.SetFloat("Speed", transformedInputDirection.magnitude);
        anim.SetBool("isSprinting?", InputProvider.SprintPressed);

        sprintDustFX.gameObject.SetActive(InputProvider.SprintPressed && IsGrounded());
    }

    public bool IsGrounded()
    {
        return Physics.CheckSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius, whatIsGround);
    }

    private void Jump()
    {
        if (IsGrounded())
        {
            ApplyImpulseForce(Vector3.up * jumpForce);

            if (jumpDustFX.isPlaying)
                jumpDustFX.Stop();

            jumpDustFX.Play();
        }
    }

    public void ApplyImpulseForce(Vector3 force, bool overrideVelocity = false)
    {
        if (overrideVelocity)
            motor.velocity = Vector3.zero;

        motor.AddForce(force, ForceMode.Impulse);
    }

    private void LoadPlayerLocation(PlayerData data)
    {
        transform.position = data.position;
        transform.rotation = data.rotation;
    }

    private void OnDrawGizmos()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.down * groundCheckOffset, groundCheckRadius);
    }
}
