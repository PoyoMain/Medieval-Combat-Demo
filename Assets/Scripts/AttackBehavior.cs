using UnityEngine;

public class AttackBehavior : StateMachineBehaviour
{
    private ICombatant iCombatant;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.gameObject.TryGetComponent(out iCombatant)) return;

        iCombatant.StartAttack();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        iCombatant?.EndAttack();
    }
}
