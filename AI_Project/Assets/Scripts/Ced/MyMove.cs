using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MyMove : MonoBehaviour
{
    //vitesse de deplacement
    public float walkSpeed;
    public float runSpeed;
    public float turnSpeed;

    //Input
    public string inputFront;
    public string inputBack;
    public string inputLeft;
    public string inputRight;

    public Vector3 jumbSpeed;
    CapsuleCollider playerCollider;

    // Start is called before the first frame update
    void Start()
    {
        playerCollider = gameObject.GetComponent<CapsuleCollider>();
    }

    // Update is called once per frame
    void Update()
    {
        //walk
        if (Input.GetKey(inputFront) && !Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, 0, walkSpeed * Time.deltaTime);
        }

        //run
        if (Input.GetKey(inputFront) && Input.GetKey(KeyCode.LeftShift))
        {
            transform.Translate(0, 0, runSpeed * Time.deltaTime);
        }

        //recule
        if (Input.GetKey(inputBack))
        {
            transform.Translate(0, 0, -(walkSpeed / 2) * Time.deltaTime);
        }

        //gauche
        if (Input.GetKey(inputLeft))
        {
            transform.Rotate(0, -turnSpeed * Time.deltaTime, 0);
        }

        //droite
        if (Input.GetKey(inputRight))
        {
            transform.Rotate(0, turnSpeed * Time.deltaTime, 0);
        }

        //saut
        if (Input.GetKeyDown(KeyCode.Space) && IsGrounded())
        {
            Vector3 v = gameObject.GetComponent<Rigidbody>().velocity;
            v.y = jumbSpeed.y;

            gameObject.GetComponent<Rigidbody>().velocity = jumbSpeed;
        }
    }

    bool IsGrounded()
    {
        Vector3 dwn = transform.TransformDirection(Vector3.down);

        return (Physics.Raycast(transform.position, dwn, 0.7f));
    }

}
