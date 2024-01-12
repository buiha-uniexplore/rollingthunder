using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NewMoveScript : MonoBehaviour
{
    [Header("Object")]
    public GameObject robot;
    [Header("Movement")]
    public float moveSpeed;
    [Header("Test")]
    public float test1;
    public float test2;


    private float horizontalInput;
    private float verticalInput;

    private Vector3 moveDirection;
    private Vector3 fallDirection;

    [SerializeField]
    private GameObject dialogueManager;

    Rigidbody rb;

    // Start is called before the first frame update
    void Start()
    {
        rb = GetComponent<Rigidbody>();
        rb.freezeRotation = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        MyInput();

        MovePlayer();

        
    }

    private void MyInput()
    {
        horizontalInput = Input.GetAxisRaw("Horizontal");
        verticalInput = Input.GetAxisRaw("Vertical");

    }

    private void MovePlayer()
    {
        //moveDirection = Vector3.forward * verticalInput + Vector3.right * horizontalInput;
        moveDirection = transform.forward * verticalInput + transform.right * horizontalInput;
        //rb.velocity = moveDirection.normalized * moveSpeed * 10f;
        rb.AddForce(moveDirection.normalized * moveSpeed * 10f, ForceMode.Force);

        int layerMask = 1 << 3; //Terrain layer

        RaycastHit hit;
        if (Physics.Raycast(robot.transform.position, Vector3.down, out hit, Mathf.Infinity, layerMask))
        {
            SphereCollider robotCollider = robot.GetComponent<SphereCollider>();
            float robotScale = robot.transform.localScale.x; 
            float pivotOffSet = robotCollider.radius*robotScale;
            float distanceToGround = hit.distance - pivotOffSet;
            /*            Debug.Log("distance from pivot = " + hit.distance.ToString());
                        Debug.Log("distance to ground = " + distanceToGround.ToString());
                        Debug.Log("hit.point" + hit.point);*/
            transform.position = hit.point + new Vector3(0, pivotOffSet, 0);

        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Pillar"))
        {
            rb.velocity = new Vector3(0f, 0f, 0f);
        }
    }

    public void SetMovementInactive()
    {
        moveSpeed = 0;
    }
}
