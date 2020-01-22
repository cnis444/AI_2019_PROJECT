using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Climbable : InteractiveObject
{
    public override void DoSomething() {
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<PlayerMovement>().isClimbing = true;
    }
}
