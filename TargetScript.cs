using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetScript : MonoBehaviour
{
    [SerializeField]
    private GameObject littleGhost;
    public GameObject explosionEffect;

    [SerializeField]
    private int health = 50;
    private EnemyData enemyData;

    private void Start()
    {
        enemyData = gameObject.GetComponent<EnemyData>();
    }

    public EnemyData GetData()
    {
        return enemyData;
    } 

    public bool TakeDamage(int amount)
    {
        health -= amount;
        if(health <= 0)
        {
            Die();
            return false;
        }
        return true;
    }

    public void Die()
    {
        littleGhost.SetActive(false);
        gameObject.GetComponent<CapsuleCollider>().enabled = false;
        gameObject.GetComponent<Rigidbody>().isKinematic = false;

        GameObject obj = Instantiate(explosionEffect, gameObject.transform.position, Quaternion.identity);
        obj.GetComponent<ParticleSystem>().Play();
        Destroy(obj, 2f);
        Destroy(gameObject, 2f);

    }

    IEnumerator WaitAndDestroy(ParticleSystem prefab, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(prefab);
        Destroy(gameObject);
    }
}
