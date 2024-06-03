using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseMovement : MonoBehaviour
{
    public float mouseSensitivity = 150f;
    public float topClamp = -90f;
    public float bottomClamp = 90f;
    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        // Center the cursor and hide it
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        // Get mouse inputs
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;
        // Mouse rotations X axis ( Up & Down )
        xRotation -= mouseY;
        // Rotation threshold 
        xRotation = Mathf.Clamp(xRotation, topClamp, bottomClamp);
        // Mouse rotations Y axis ( Left & Right )
        yRotation += mouseX;
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }
}
