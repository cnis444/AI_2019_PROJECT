using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FreeCamLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;

    // Start is called before the first frame update
    void Start() {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update() {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        Vector3 move = Vector3.up * mouseX + transform.right * mouseY;

        //transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
        transform.Rotate(move);

        // Clamp vertical rotation
        Quaternion rotation = transform.rotation;
        rotation.y = Mathf.Clamp(rotation.y, -90f, 90f);
        transform.rotation = rotation;
    }
}
