using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Rigidbody), typeof(FieldOfView), typeof(NavMeshAgent))]
public abstract class NPC : MonoBehaviour
{
    public string myName;
    public int id;
    public float radiusCommunication;

    protected NavMeshAgent agent;
    protected Rigidbody myRigidbody;
    protected Vector3 destination;

    public virtual void Start()
    {
        myRigidbody = gameObject.GetComponent<Rigidbody>();
        agent = gameObject.GetComponent<NavMeshAgent>();
    }


    public virtual Vector3 UpdateMove()
    {
        return new Vector3();
    }

    public virtual void NextAction()
    {

    }

}
