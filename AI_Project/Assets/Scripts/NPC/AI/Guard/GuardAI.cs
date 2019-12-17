using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class GuardAI : NPC
{

    Vector3 startPos;
    int onSightHash;
    int reachStartHash;

    public Vector3 GuardStartPosition { get { return startPos; } }

    public int ReachStartHash { get { return reachStartHash; } }

    // Start is called before the first frame update
    public override void Start()
    {
        base.Start();
        startPos = transform.position;
        onSightHash = Animator.StringToHash("PlayerOnSight");
        reachStartHash = Animator.StringToHash("ReachStarPos");
    }

    // Update is called once per frame
    void Update()
    {
        behavior.SetBool(onSightHash, fov.visibleTargets.Count != 0);
    }
}
