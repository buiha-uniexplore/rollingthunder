using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MagnetScript : MonoBehaviour
{
    private float robotRadius;
    [SerializeField]
    private float suckSpeed;
    // Start is called before the first frame update
    void Start()
    {
        GameObject robot = GameObject.Find("robot");
        robotRadius = robot.GetComponent<SphereCollider>().radius;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Bullet"))
        {
            Vector3 robotPosition = gameObject.transform.position;
            Vector3 direction = new Vector3(robotPosition.x, robotPosition.y, robotPosition.z) - other.gameObject.transform.position;
            Rigidbody bulletRb = other.gameObject.GetComponent<Rigidbody>();
            bulletRb.velocity = direction * suckSpeed;
        }
    }
}
