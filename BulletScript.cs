using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletScript : MonoBehaviour
{
    [SerializeField]
    private GameObject dialogueManager;
    [SerializeField]
    private bool isOnGround = false;
    [SerializeField]
    private float altitude;
    [SerializeField]
    private float frequency;
    [SerializeField]
    private float offset;
    [SerializeField]
    private float offsetSeconds;
    [SerializeField]
    private float flyUpSpeed;
    private float positionY;
    [SerializeField]
    private bool isCollidingWithPlayer = false;
    private Rigidbody rb;
    private bool firstCollectBullet = false;
    private Vector3 constantPositionOnGround;


    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("Dialogue Manager");
        rb = gameObject.GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {

        if (isOnGround)
        {
            if (!isCollidingWithPlayer)
            {

                Vector3 tempPosition = constantPositionOnGround;

                positionY = tempPosition.y;

                //Debug.Log(Mathf.Sin(Time.fixedTime * frequency) * altitude);
                positionY += Mathf.Sin(Time.fixedTime * frequency) * altitude;

                tempPosition.y = positionY;

                gameObject.transform.position = tempPosition;
            }
            else
            {

            }

            
        }
        else
        {
            RaycastHit hit;
            int layerMask = 1 << 3;

            if (Physics.Raycast(gameObject.transform.position, Vector3.down, out hit, offset, layerMask))
            {
                constantPositionOnGround = gameObject.transform.position;
                rb.useGravity = false;
                rb.isKinematic = true;
                isOnGround = true;
                rb.isKinematic = false;
            }
        }      
    }

    private void OnTriggerEnter(Collider other)
    {
        if (isOnGround)
        {
            if (other.gameObject.CompareTag("Player"))
            {
                isCollidingWithPlayer = true;
                if (!firstCollectBullet && dialogueManager.GetComponent<ExplainationScript>().BulletText == false)
                {
                    dialogueManager.GetComponent<ExplainationScript>().RunNearBullets = true;
                    firstCollectBullet = true;
                }

            }
        }
        
    }
}
