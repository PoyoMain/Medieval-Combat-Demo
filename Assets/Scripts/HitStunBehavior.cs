using UnityEngine;

public class HitStunBehavior : StateMachineBehaviour
{
    private ICombatant iCombatant;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.gameObject.TryGetComponent(out iCombatant)) return;

        iCombatant.StartHitStun();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        iCombatant?.EndHitStun();
        animator.ResetTrigger("Hit");
    }
}
