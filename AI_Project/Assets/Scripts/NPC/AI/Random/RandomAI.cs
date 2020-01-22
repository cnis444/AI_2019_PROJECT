using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;


public class RandomAI : NPC
{

    [Range(2, 15)]
    public float delay;
    public int moveRange;

    public override void Start()
    {
        base.Start();
        StartCoroutine("Move", delay);
    }

    public Vector3 NextDst()
    {
        Vector3 dst = new Vector3(Random.Range(-moveRange, moveRange), 0, Random.Range(-moveRange, moveRange));
        return dst;
    }

    IEnumerator Move(float delay)
    {
        agent.SetDestination(NextDst());
        while (true)
        {
            yield return new WaitForSeconds(delay);
            agent.SetDestination(NextDst());
            
        }
    }

}
