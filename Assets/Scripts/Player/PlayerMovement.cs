using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator anim;
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
        Vector2 input = InputProvider.MovementInput;

        transformedInputDirection = new Vector3(input.x, 0f, input.y);

        if (transformedInputDirection.magnitude > 1f)
            transformedInputDirection.Normalize();

        HandleAnimations();

        motor.drag = IsGrounded() ? groundedDrag : airDrag;
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
            transform.forward = Vector3.Slerp(transform.forward, transformedInputDirection, Time.deltaTime * turnSpeed);

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
