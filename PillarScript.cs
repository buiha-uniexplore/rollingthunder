using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PillarScript : MonoBehaviour
{
    
    public GameObject bulletDrop;
    private Vector3 topCoordinate;
    [SerializeField]
    int spawnBulletNumber;
    [SerializeField]
    private ColorScript.ColorType pillarColor;
    public ColorScript.ColorType PillarColor
    {
        get { return pillarColor; }
        set { pillarColor = value; }
    }
    [SerializeField]
    private float bulletSpawnForce;
    [SerializeField]
    private GameObject dialogueManager;
    private bool hitPillarOnce = false;
    private int collisionCount = 0;
    public int CollisionCount
    {
        get { return collisionCount; }
    }

    // Start is called before the first frame update
    void Start()
    {
        dialogueManager = GameObject.Find("Dialogue Manager");

        topCoordinate = gameObject.GetComponent<MeshFilter>().mesh.bounds.max;
        topCoordinate.x = gameObject.transform.position.x;
        topCoordinate.z = gameObject.transform.position.z;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter(Collision collision)
    {
        collisionCount++;
        if (collision.gameObject.CompareTag("Player"))
        {
            SpawnBullets();

            if (!hitPillarOnce)
            {
                dialogueManager.GetComponent<ExplainationScript>().HitPillar = true;
                hitPillarOnce = true;
            }

        }else if (collision.gameObject.CompareTag("Terrain"))
        {
            

        }
    }

    private void OnCollisionExit(Collision collision)
    {
        collisionCount--;
    }

    public void SpawnBullets()
    {
        for(int i = 0; i < spawnBulletNumber; i++)
        {
            topCoordinate.x += Random.Range(-0.1f, 0.1f);
            topCoordinate.z += Random.Range(-0.1f, 0.1f);

            var obj = Instantiate(bulletDrop, topCoordinate, Quaternion.identity);
            obj.GetComponent<BulletData>().BulletColor = pillarColor;

            float addForceX = Random.Range(-1f, 1f);
            float addForceZ = Random.Range(-1f, 1f);
            //Debug.Log(addForceZ + " " + addForceX);
            obj.GetComponent<Rigidbody>().AddForce(new Vector3(addForceX, 1 * bulletSpawnForce, addForceZ), ForceMode.Impulse);
        }
        
    }
}
