using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform playerBody;
    private float xRotation = 0f;
    private float yRotation = 0f;
    // Start is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        xRotation -= mouseY;
        yRotation -= mouseX;
        xRotation = Mathf.Clamp(xRotation, -50f, 50f);

        //playerBody.localRotation = Quaternion.Euler(xRotation, mouseX, 0f);
        //playerBody.Rotate(Vector3.right * mouseY);
        //playerBody.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        //playerBody.Rotate(mouseY, mouseX, 0);
        //playerBody.Rotate(mouseY, 0, 0);



    }
}
