using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewLookScript : MonoBehaviour
{
    [Header("Mouse")]
    public float mouseSens;

    [Header("Rotation")]
    public float rotSpeed;

    public float mouseX;
    public float mouseY;
    public float rotX;
    public float rotY;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    private void FixedUpdate()
    {
        RotateObject();
    }

    private void MyInput()
    {
        mouseX = Input.GetAxis("Mouse X") * mouseSens * Time.deltaTime;
        mouseY = Input.GetAxis("Mouse Y") * mouseSens * Time.deltaTime;

    }

    private void RotateObject()
    {
        rotX += mouseY;
        rotY += mouseX;
        rotX = Mathf.Clamp(rotX, -45, 45);
        //transform.Rotate(0, mouseX, 0, Space.Self);
        transform.rotation = Quaternion.Euler(-rotX, rotY, 0);
        //rb.AddTorque(transform.up * rotSpeed * mouseX);
        
    }

    public void SetLookInactive()
    {
        rotSpeed = 0;
        mouseSens = 0;
    }
}
