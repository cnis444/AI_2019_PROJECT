using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAIGoBackBehavior : StateMachineBehaviour
{
    private GuardAI guard;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (guard == null)
        {
            guard = animator.gameObject.GetComponent<GuardAI>();
        }
        guard.Agent.SetDestination(guard.GuardStartPosition);
    }
    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (guard.Agent.remainingDistance < 0.6f)
        {
            animator.SetTrigger(guard.ReachStartHash);
        }
    }

}
