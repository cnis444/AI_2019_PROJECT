using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GuardSearchPlayerBehavior : StateMachineBehaviour
{
    private GuardAI guard;
    private float time;
    private float currentTime;
    private Vector3 currentPos;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (guard == null)
        {
            guard = animator.gameObject.GetComponent<GuardAI>();
        }
        time = Time.time;
        currentTime = guard.persistance / 5f;
        currentPos = animator.gameObject.transform.position;

    }

    public override void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if ( Time.time > guard.persistance + time)
        {
            animator.SetTrigger(guard.SearchTimeUpHash);
        }

        else if(currentTime < 0)
        {
            guard.Agent.SetDestination(currentPos + new Vector3(Random.Range(-10, 10), 0, Random.Range(-10, 10)));
            currentTime = guard.persistance / 5f;
        }
        else
        {
            currentTime -= Time.deltaTime;
        }


    }
}
