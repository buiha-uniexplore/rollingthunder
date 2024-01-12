using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RobotScript : MonoBehaviour
{
    private float speed = 5.0f;
    private CharacterController controller;

    // Start is called before the first frame update
    void Start()
    {
        controller = gameObject.GetComponent<CharacterController>();    
    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
    }

    void MyInput()
    {
        Vector3 move = transform.right*Input.GetAxis("Horizontal") + transform.forward *Input.GetAxis("Vertical");
        controller.Move(move * Time.deltaTime * speed);



    }
}
