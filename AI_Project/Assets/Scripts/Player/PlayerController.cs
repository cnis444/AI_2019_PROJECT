using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour
{
    public float walkSpeed = 1;
    public float runSpeed = 2;
    public float jumpSpeed = 2;

    public float gravity = 9.81f;
    private float vSpeed = 0;

    private CharacterController character;
    private Camera m_camera;

    // Start is called before the first frame update
    void Start()
    {
        character = GetComponent<CharacterController>();
        m_camera = Camera.main;
    }

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move() {
        Vector3 movement = Vector3.zero;
        
        if (Input.GetKey(KeyCode.LeftShift)) {
            movement += Run();
        }
        else {
            movement += Walk();
        }

        if (character.isGrounded) {
            vSpeed = 0;
            Jump();
        }
        else {
            vSpeed -= gravity * Time.deltaTime;
        }
        movement += vSpeed * transform.up;

        character.Move(movement);
    }

    Vector3 Walk(float speed) {
        Vector3 dir = Vector3.Normalize(transform.forward * Input.GetAxis("Horizontal") + transform.right * Input.GetAxis("Vertical"));
        return dir * speed;
    }

    Vector3 Walk() {
        return Walk(walkSpeed);
    }

    Vector3 Run() {
        return Walk(runSpeed);
    }

    void Jump() {
        if (Input.GetKey(KeyCode.Space)) {
            vSpeed = jumpSpeed;
        }
    }
}
