using UnityEngine;

[RequireComponent(typeof(Rigidbody), typeof(ColliderActivator), typeof(Animator))]
public class Enemy : MonoBehaviour, ICombatant
{
    [Header("Move Stats")]
    [SerializeField] private float speed;

    [Header("Scout Stats")]
    [SerializeField] private float scoutMinTime;
    [SerializeField] private float scoutMaxTime;

    [Header("Wait Stats")]
    [SerializeField] private float waitTime;

    [Header("Approach Stats")]
    [SerializeField] private Transform target;
    [SerializeField] private float targetDistance;
    [SerializeField] private float maxApproachTime;

    private State state;

    private Animator anim;

    private void Awake()
    {
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
                anim.SetFloat("MoveInputX", 0);
                anim.SetFloat("MoveInputY", 0);
                break;
            case State.DoingAction:
                int choice = Random.Range(0,2) == 0 ? 0 : 1;

                if (choice == 0) Attack();
                else Block();
                break;
        }
    }

    #endregion

    #region Scouting

    private float scoutTimer;
    private ScoutDirection scoutDirection;

    private void HandleScouting()
    {

    }

    #endregion

    #region Waiting

    private float waitTimer;

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
            }
        }

        float distanceToPlayer = Vector3.Distance(transform.position, target.position);

        if (distanceToPlayer > targetDistance)
        {
            Vector3 directionToPlayer = (target.position - transform.position).normalized;
            Vector3 moveVector = new(Mathf.Abs(directionToPlayer.x), 0, Mathf.Abs(directionToPlayer.z));
            transform.Translate(speed * Time.deltaTime * moveVector, Space.Self);

            anim.SetFloat("MoveInputX", moveVector.x);
            anim.SetFloat("MoveInputY", moveVector.z);
        }
        else
        {
            ChangeState(State.DoingAction);
        }

        print(distanceToPlayer);
    }

    #endregion

    #region Actions

    private bool isAttacking;
    private bool isBlocking;

    private void Attack()
    {
        anim.SetTrigger("Attack");
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
    }

    public void EndBlock()
    {
        isBlocking = false;
        ChangeState(State.Waiting);
    }

    #endregion

    #region Targeting

    private void HandleTargeting()
    {
        Vector3 targetPosition = new(target.position.x, transform.position.y, target.position.z);
        transform.LookAt(targetPosition, Vector3.up);
    }

    #endregion

    private enum State { Scouting, Approaching, Waiting, DoingAction}
    private enum ScoutDirection { Left, Right, Back}
}
