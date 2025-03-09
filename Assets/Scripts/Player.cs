using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody))]
public class Player : MonoBehaviour
{
    [Header("Movement")]
    [SerializeField] private float movementSpeed;
    [SerializeField] private float movementAcceleration;

    [Header("Looking")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetingAcceleration;

    private PlayerControls playerControls;
    private PlayerControls.GameplayControlsActions controls;

    private Vector2 moveInput;
    private Vector3 velocity;

    private Rigidbody rb;

    private void Awake()
    {
        TryGetComponent(out rb);
    }

    private void OnEnable()
    {
        playerControls = new();
        controls = playerControls.GameplayControls;
        controls.Enable();
    }

    private void Update()
    {
        GetInput();
    }

    private void GetInput()
    {
        moveInput = controls.Move.ReadValue<Vector2>();
    }

    private void FixedUpdate()
    {
        HandleMovement();
        HandleTargeting();

        ApplyVelocity();
    }

    private void HandleMovement()
    {
        Vector3 moveVector = new(moveInput.x, 0, moveInput.y);
        transform.Translate(movementSpeed * Time.deltaTime * moveVector, Space.Self);
    }

    private void HandleTargeting()
    {
        Vector3 targetPosition = new(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition, Vector3.up);
    }

    private void ApplyVelocity() => rb.linearVelocity = velocity; 

    private void OnDisable()
    {
        controls.Disable();
    }
}
