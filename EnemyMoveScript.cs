using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyMoveScript : MonoBehaviour
{
    public GameObject player;
    private Rigidbody rb;
    private EnemyData enemyData;
    private TargetScript targetScript;

    private PlayerData playerData;

    private Vector3 moveDirection;
    
    public float knockBackTime = 0f;
    [SerializeField]
    private float knockBackForce = 2f;
    [SerializeField]
    private int scoreOff = 10;
    [SerializeField]
    private int healthOff = 1;
    public GameObject healthGameObject;

    
    private bool canBeHit = false;
    // Start is called before the first frame update
    void Start()
    {
        rb = gameObject.GetComponent<Rigidbody>();
        enemyData = gameObject.GetComponent<EnemyData>();
        targetScript = gameObject.GetComponent<TargetScript>();

        player = GameObject.Find("Player");
        playerData = player.GetComponent<PlayerData>();
        healthGameObject = GameObject.Find("Health");
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        if(enemyData.GetColor() == playerData.GetColor())
        {
            canBeHit = true;
        }
        Move();
    }

    public void Move()
    {
        if (knockBackTime <= 0)
        {
            rb.velocity = (player.transform.position - gameObject.transform.position)/5;
            moveDirection = rb.velocity.normalized;
            transform.rotation = Quaternion.LookRotation(moveDirection, Vector3.up);
        }
        else
        {
            KnockBack();
        }
    }

    public void KnockBack()
    {
        Debug.Log("Knocking back");
        knockBackTime -= Time.deltaTime;
        Vector3 knockBackDirection = gameObject.transform.position - player.transform.position;
        rb.velocity = knockBackDirection.normalized*knockBackForce;

    }

    public void ChangeCanBeHitStatus(bool status)
    {
        canBeHit = status;
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            int playerScore = playerData.GetScore();
            playerData.SetScore(playerScore - scoreOff, out playerScore);
            playerData.Health -= healthOff;
            healthGameObject.GetComponent<HealthScript>().DecreaseHealth();
            targetScript.Die();
        } 
    }
}
