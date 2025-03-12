using System.Drawing;
using System.Threading;
using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ColliderActivator), typeof(Animator))]
public class Enemy : MonoBehaviour, ICombatant
{
    [Header("Move Stats")]
    [SerializeField] private float speed;


    [Header("Wait Stats")]
    [SerializeField] private float waitTime;

    [Header("Approach Stats")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetDistance;
    [SerializeField] private float maxApproachTime;

    [Header("Hurtbox")]
    [SerializeField] private Collider hurtboxCollider;

    private State state;

    private Rigidbody rb;
    private Animator anim;

    private void Awake()
    {
        TryGetComponent(out rb);
        TryGetComponent(out anim);

        ChangeState(State.Waiting);
    }

    private void FixedUpdate()
    {
        HandleTargeting();

        if (state == State.Waiting)
        {
            HandleWaiting();
        }
        else if (state == State.Approaching)
        {
            HandleApproaching();
        }
        else if (state == State.InHitStun)
        {
            HandleHitStun();
        }
    }

    #region State Changing

    private void ChangeState(State newState)
    {
        state = newState;

        switch (state)
        {
            case State.Scouting:
                break;
            case State.Approaching:
                approachTimer = maxApproachTime;
                break;
            case State.Waiting:
                waitTimer = waitTime;
                rb.linearVelocity = Vector3.zero;
                anim.SetFloat("MoveInputX", 0);
                anim.SetFloat("MoveInputY", 0);
                break;
            case State.DoingAction:
                rb.linearVelocity = Vector3.zero;
                anim.SetFloat("MoveInputX", 0);
                anim.SetFloat("MoveInputY", 0);
                int choice = Random.Range(0, 2) == 0 ? 0 : 1;

                if (choice == 0) Attack();
                else Block();
                break;
            case State.InHitStun:
                break;
        }
    }

    #endregion

    #region Waiting

    private float waitTimer;
    private bool IsWaiting => waitTimer > 0;

    private void HandleWaiting()
    {
        if (waitTimer > 0)
        {
            waitTimer -= Time.fixedDeltaTime;

            if (waitTimer <= 0)
            {
                ChangeState(State.Approaching);
            }
        }
    }

    #endregion

    #region Approaching

    private float approachTimer;

    private void HandleApproaching()
    {
        if (approachTimer > 0)
        {
            approachTimer -= Time.fixedDeltaTime;

            if (approachTimer <= 0)
            {
                ChangeState(State.Waiting);
                return;
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer > targetDistance)
        {
            rb.linearVelocity = transform.forward * speed;

            anim.SetFloat("MoveInputX", 0);
            anim.SetFloat("MoveInputY", 1);
        }
        else
        {
            ChangeState(State.DoingAction);
        }
    }

    #endregion

    #region Actions

    private bool isAttacking;
    private bool isBlocking;
    public bool IsBlocking => isBlocking;

    private void Attack()
    {
        int choice = Random.Range(0, 2);
        
        if (choice == 0) anim.SetTrigger("LightAttack");
        else anim.SetTrigger("HeavyAttack");
    }

    private void Block()
    {
        anim.SetTrigger("Block");
    }

    public void StartAttack()
    {
        isAttacking = true;
    }

    public void EndAttack()
    {
        isAttacking = false;
        ChangeState(State.Waiting);
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
        ChangeState(State.Waiting);
    }

    #endregion

    #region Hit

    private bool inHitStun;

    public void Hit()
    {
        anim.SetTrigger("Hit");
        anim.SetFloat("MoveInputX", 0);
        anim.SetFloat("MoveInputY", 0);
    }

    public void ShieldHit()
    {
        anim.SetTrigger("ShieldHit");
        int choice = Random.Range(0, 2);
        if (choice == 0) anim.SetTrigger("LightAttack");
    }

    public void HandleHitStun()
    {
        if (inHitStun)
        {

        }
    }

    public void StartHitStun()
    {
        inHitStun = true;
    }

    public void EndHitStun()
    {
        inHitStun = false;
    }

    #endregion

    #region Targeting

    private void HandleTargeting()
    {
        if (isAttacking || IsWaiting) return;
        Vector3 targetposition = new(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetposition, Vector3.up);
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

    private enum State { Scouting, Approaching, Waiting, DoingAction, InHitStun}
    private enum ScoutDirection { Left, Right, Back}
}
