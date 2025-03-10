using UnityEngine;

public class BlockBehavior : StateMachineBehaviour
{
    private ICombatant iCombatant;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (!animator.gameObject.TryGetComponent(out iCombatant)) return;

        iCombatant.StartBlock();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        iCombatant?.EndBlock();
    }
}
