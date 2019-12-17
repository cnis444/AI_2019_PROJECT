using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Rigidbody), typeof(FieldOfView), typeof(NavMeshAgent))]
public abstract class NPC : MonoBehaviour
{
    public NPCUI infoCanvas;

    protected string myName;
    protected int id;
    protected float radiusCommunication;

    protected Rigidbody myRigidbody;
    protected FieldOfView fov;
    protected NavMeshAgent agent;
    protected Animator behavior;

    public virtual void Start()
    {

        myRigidbody = gameObject.GetComponent<Rigidbody>();
        fov = gameObject.GetComponent<FieldOfView>();
        agent = gameObject.GetComponent<NavMeshAgent>();
        behavior = gameObject.GetComponent<Animator>();
        GenerateParam();
        
    }

    public virtual void GenerateParam()
    {
        id = AIParam.NextId();
        myName = AIParam.RandomName();
        radiusCommunication = AIParam.RadiusCommunication();
        infoCanvas.SetInfo(myName, "");
        infoCanvas.SetStarLevel(AIParam.StartLevel());


    }

    #region GETTER
    public virtual NPCUI InfoCanvas { get { return infoCanvas; } }

    public virtual float RadiusCommunication { get { return radiusCommunication; } }

    public virtual string Name { get { return myName; } }

    public virtual int Id { get { return id; } }

    public virtual Rigidbody Body { get { return myRigidbody; } }

    public virtual FieldOfView Fov { get { return fov; } }

    public virtual NavMeshAgent Agent {get { return agent; } }

    public virtual Animator Behavior { get { return behavior; } }

    #endregion

}
