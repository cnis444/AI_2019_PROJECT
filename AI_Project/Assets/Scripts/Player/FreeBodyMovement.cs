using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeBodyMovement : MonoBehaviour
{
    public float speed;

    // Update is called once per frame
    void Update()
    {
        Move();
    }

    void Move() {
        float x = Input.GetAxis("Horizontal");
        float y = Input.GetAxis("Height");
        float z = Input.GetAxis("Vertical");

        Vector3 move = transform.right * x + transform.up * y + transform.forward * z;
        move = move.normalized;
        move = move * Mathf.Max(Mathf.Abs(x), Mathf.Abs(y), Mathf.Abs(z));

        //transform.Translate(move * Time.deltaTime);
        transform.position += move * speed * Time.deltaTime;
    }
}
