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

    [Header("Hurtbox")]
    [SerializeField] private Collider hurtboxCollider;

    private PlayerControls playerControls;
    private PlayerControls.GameplayControlsActions controls;

    private Vector2 moveInput;
    private Vector3 velocity;
    private bool lightAttackDown;
    private bool heavyAttackDown;
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
        lightAttackDown = controls.LightAttack.WasPressedThisFrame();
        heavyAttackDown = controls.HeavyAttack.WasPressedThisFrame();
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
        if (isAttacking) return;

        if (lightAttackDown) anim.SetTrigger("LightAttack");
        else if (heavyAttackDown) anim.SetTrigger("HeavyAttack");
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
    public bool IsBlocking => isBlocking;
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
        anim.SetBool("Blocking", true);
    }

    public void EndBlock()
    {
        isBlocking = false;
        anim.SetBool("Blocking", false);
    }

    #endregion

    #region Movement

    private void HandleMovement()
    {
        if (isAttacking || inHitStun) return;

        Vector3 moveVector = new(moveInput.x, 0, moveInput.y);
        rb.AddRelativeForce(moveVector * movementSpeed, ForceMode.Force);

        anim.SetFloat("MoveInputX", moveInput.x);
        anim.SetFloat("MoveInputY", moveInput.y);
    }

    #endregion

    #region Targetting

    private void HandleTargeting()
    {
        if (isAttacking) return;
        Vector3 targetPosition = new(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition, Vector3.up);
    }

    #endregion

    #region Hit

    private bool inHitStun;

    public void Hit()
    {
        anim.SetTrigger("Hit");
    }

    public void StartHitStun()
    {
        inHitStun = true;
    }

    public void EndHitStun()
    {
        inHitStun = false;
    }

    public void ShieldHit()
    {
        anim.SetTrigger("ShieldHit");
    }

    #endregion

    #region Die

    public void Die()
    {
        anim.SetTrigger("Die");
        hurtboxCollider.enabled = false;
        this.enabled = false;
    }

    #endregion

    private void ApplyVelocity() => rb.linearVelocity = velocity; 

    private void OnDisable()
    {
        controls.Disable();
    }
}
