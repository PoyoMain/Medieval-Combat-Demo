using System;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ColliderActivator), typeof(Animator))]
public class Player : MonoBehaviour, ICombatant
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
    private bool attackDown;
    private bool blockDown;

    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out anim);
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
        attackDown = controls.Attack.WasPressedThisFrame();
        blockDown = controls.Block.WasPressedThisFrame();
    }

    private void FixedUpdate()
    {
        HandleAttacking();
        HandleBlocking();

        HandleMovement();
        HandleTargeting();

        ApplyVelocity();
    }

    #region Attacking

    private bool isAttacking;

    private void HandleAttacking()
    {
        if (attackDown && !isAttacking && !isBlocking)
        {
            anim.SetTrigger("Attack");
        }
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
    }

    #endregion

    #region Blocking

    private bool isBlocking;

    private void HandleBlocking()
    {
        if (blockDown && !isBlocking && !isAttacking)
        {
            anim.SetTrigger("Block");
        }
    }

    public void StartBlock()
    {
        isBlocking = true;
    }

    public void EndBlock()
    {
        isBlocking = false;
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        if (isAttacking) return;

        Vector3 moveVector = new(moveInput.x, 0, moveInput.y);
        transform.Translate(movementSpeed * Time.deltaTime * moveVector, Space.Self);

        anim.SetFloat("MoveInputX", moveInput.x);
        anim.SetFloat("MoveInputY", moveInput.y);
    }

    #endregion

    #region Targetting

    private void HandleTargeting()
    {
        Vector3 targetPosition = new(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition, Vector3.up);
    }

    #endregion

    private void ApplyVelocity() => rb.linearVelocity = velocity; 

    private void OnDisable()
    {
        controls.Disable();
    }
}
