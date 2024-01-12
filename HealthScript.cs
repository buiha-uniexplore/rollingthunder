using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealthScript : MonoBehaviour
{
    public GameObject healthGameObject;
    [SerializeField]
    private GameObject heartPrefab;
    private int maxHealth;
    [SerializeField]
    private float increment;
    [SerializeField]
    private GameObject player;
    [SerializeField]
    private List<GameObject> hearts = new List<GameObject>();
    
    private PlayerData playerData;

    // Start is called before the first frame update
    void Start()
    {
        playerData = player.GetComponent<PlayerData>();
        maxHealth = playerData.Health;
        InitializeHearts();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void InitializeHearts()
    {
        Debug.Log("Initializing hearts");
        if(maxHealth%2== 0)
        {
            for (int i = -Mathf.FloorToInt(maxHealth / 2); i < Mathf.FloorToInt(maxHealth / 2); i++)
            {
                var heart = Instantiate(heartPrefab, gameObject.transform.position + new Vector3(increment, 0, 0) * i, Quaternion.identity);
                heart.transform.SetParent(healthGameObject.transform);
                heart.transform.localPosition = new Vector3(increment / 2, 0, 0) + new Vector3(increment, 0, 0) * i;
                heart.transform.localScale = new Vector3(1, 1, 1);
                hearts.Add(heart);
            }
        }
        else
        {
            for (int i = -Mathf.FloorToInt(maxHealth / 2); i < Mathf.FloorToInt(maxHealth / 2) + 1; i++)
            {
                var heart = Instantiate(heartPrefab, gameObject.transform.position + new Vector3(increment, 0, 0) * i, Quaternion.identity);
                heart.transform.SetParent(healthGameObject.transform);
                heart.transform.localPosition = new Vector3(0, 0, 0) + new Vector3(increment, 0, 0) * i;
                heart.transform.localScale = new Vector3(1, 1, 1);
                hearts.Add(heart);
            }
        }
              
    }

    public void DecreaseHealth()
    {
        if(hearts.Count > 0)
        {
            Destroy(hearts[hearts.Count - 1]);
            hearts.RemoveAt(hearts.Count - 1);
        }
        
    }
}
