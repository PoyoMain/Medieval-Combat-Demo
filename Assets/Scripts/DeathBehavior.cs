using UnityEngine;

public class DeathBehavior : StateMachineBehaviour
{
    [SerializeField] private VoidEventSO onDeathStartEvent;
    [SerializeField] private VoidEventSO onDeathEvent;

    public override void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onDeathStartEvent.RaiseEvent();
    }

    public override void OnStateExit(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        onDeathEvent.RaiseEvent();
    }
}