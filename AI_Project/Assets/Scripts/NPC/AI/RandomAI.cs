using System.Collections;
using System.Collections.Generic;
using UnityEngine;


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

    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.S))
        {
            StopCoroutine("Move");
        }

    }


    public override Vector3 UpdateMove()
    {
        Vector3 dst = new Vector3(Random.Range(-moveRange, moveRange), 0, Random.Range(-moveRange, moveRange));
        Debug.Log(dst);
        return dst;
    }

    IEnumerator Move(float delay)
    {

        while (true)
        {
            yield return new WaitForSeconds(delay);
            agent.SetDestination(UpdateMove());
            
        }
    }

}
