using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class SpawnScript : MonoBehaviour

{
    [SerializeField]
    private GameObject player;
    public Slider spawnTimeSlider;
    public Canvas canvas;
    [SerializeField]
    private GameObject waveTextPrefab;

    [SerializeField, Header("Spawn settings")]
    private int targetSpawnTimes;
    [SerializeField]
    private float targetSpawnInterval;
    [SerializeField]
    private float targetSpawnRadius;
    [SerializeField]
    private int targetSpawnNumber;
    [SerializeField]
    private Vector3 targetSpawnCenter;
    
    

    [SerializeField]
    private List<GameObject> targets = new List<GameObject>();
    [SerializeField]
    private List<int> targetSpawnNumbers = new List<int>();
    [SerializeField]
    private List<float> spawnTime = new List<float>();
    private float currentSpawnTime;
    private float currentTimeLeft;
    private bool runSpawnSlider = true;
    public bool RunSpawnSlider
    {
        set { runSpawnSlider = value; }
    }
    

    private delegate void TargetSpawnDelegate(Vector3 center, float radius, int targetSpawnNuber);
    private TargetSpawnDelegate spawnType;


    // Start is called before the first frame update
    void Start()
    {
        spawnType = SpawnTargetInCircle;
        IEnumerator coroutine = SpawnTargetsInSeconds(spawnType, targetSpawnInterval, targetSpawnTimes, targetSpawnRadius);
        StartCoroutine(coroutine);
    }

    private void FixedUpdate()
    {
        ProgressBar(currentSpawnTime);
    }

    void ProgressBar(float seconds)
    {
        if(currentTimeLeft > 0 && runSpawnSlider == true)
        {
            currentTimeLeft -= Time.fixedDeltaTime;
            spawnTimeSlider.value = (seconds - currentTimeLeft) / seconds;
        }
        
    }

    IEnumerator SpawnTargetsInSeconds(TargetSpawnDelegate spawnType, float seconds, int targetSpawnTimes, float radius)
    {
        for(int i = 0; i < targetSpawnTimes; i++)
        {
            currentSpawnTime = spawnTime[i];
            currentTimeLeft = currentSpawnTime;

            yield return new WaitForSeconds(spawnTime[i]);
            Vector3 playerPos = player.transform.position;
            spawnType(playerPos, radius, targetSpawnNumbers[i]);

            if(runSpawnSlider == true)
            {
                GameObject obj = Instantiate(waveTextPrefab, canvas.transform);
                obj.GetComponent<TMP_Text>().text = "Wave " + (i + 1).ToString();
                obj.transform.SetParent(canvas.transform);
                obj.GetComponent<RectTransform>().localPosition = new Vector3(140, 140, 0);
                obj.GetComponent<RectTransform>().localRotation = Quaternion.identity;
                obj.GetComponent<RectTransform>().localScale = new Vector3(1, 1, 1);
                Destroy(obj, 5f);
            }
            
        }
    }

    public void SpawnTargetInCircle(Vector3 center, float radius, int targetSpawnNumber)
    {
        Vector3 pos = new Vector3();
        for(int i = 0; i < targetSpawnNumber; i++)
        {
            float angle = Random.value * 360;

            pos.x = center.x + radius * Mathf.Cos(angle * Mathf.Deg2Rad);
            pos.y = center.y;
            pos.z = center.z + radius * Mathf.Sin(angle * Mathf.Deg2Rad);

            SpawnRandomTarget(pos);
        }

    }

    public void SpawnRandomTarget(Vector3 position)
    {
        int targetIndex = Random.Range(0,targets.Count - 1);
        var target = Instantiate(targets[targetIndex], position, Quaternion.identity);

        
    }

    IEnumerator DestroyAfterSeconds(GameObject prefab, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(prefab);
    }
}
