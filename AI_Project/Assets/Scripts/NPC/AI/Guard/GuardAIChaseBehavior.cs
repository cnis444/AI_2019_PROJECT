using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAIChaseBehavior : StateMachineBehaviour
{
    const float PathUpdateInterval = 0.3f;
    GameObject player;
    GuardAI guard;
    float lastUpdateTime = 0f;

    override public void OnStateEnter(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (player == null)
        {
            guard = animator.gameObject.GetComponent<GuardAI>();
            player = GameObject.FindGameObjectWithTag("Player");
        }
    }

    override public void OnStateUpdate(Animator animator, AnimatorStateInfo stateInfo, int layerIndex)
    {
        if (Time.time > lastUpdateTime + PathUpdateInterval)
        {
            lastUpdateTime = Time.time;
            guard.Agent.SetDestination(player.transform.position);
        }
    }
}
