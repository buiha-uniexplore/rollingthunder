using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class ExplainationScript : MonoBehaviour
{
    [SerializeField]
    private GameObject endScene;
    [SerializeField]
    private GameObject enemyA;
    [SerializeField]
    private GameObject enemyB;
    [SerializeField]
    private List<GameObject> pillars = new List<GameObject>();
    [SerializeField]
    private GameObject explainationText;
    [SerializeField]
    private List<string> dialogues = new List<string>();
    [SerializeField]
    private bool movingTextDone = false;
    [SerializeField]
    private bool hitPillar = false;
    public bool HitPillar
    {
        get { return conditions["hitPillar"]; }
        set { conditions["hitPillar"] = value; }
    }
    public bool ShootPillar
    {
        get { return conditions["shootPillar"]; }
        set { conditions["shootPillar"] = value; }
    }
    public bool MovingTextDone
    {
        get { return conditions["movingTextDone"]; }
        set { conditions["movingTextDone"] = value; }
    }
    private bool disableKey = true;
    public bool DisableKey
    {
        get { return disableKey; }
    }
    public bool RunNearBullets
    {
        set { conditions["runNearBullets"] = value; }
    }
    public bool BulletText
    {
        get { return bulletText; }
    }
    public bool ShotOnce
    {
        set { shotOnce = value; }
    }
    [SerializeField]
    private IDictionary<string, bool> conditions = new Dictionary<string, bool>();

    private GameObject currentText;

    private delegate void NextDialogueDelegate(bool? condition);

    private NextDialogueDelegate conditionToTrue;

    private float counter = 0;
    private float countDownTime = 3f;
    private bool pillarSpawn = false;
    private bool clickLeftMouse = false;
    private bool clickLeftMouseText = false;
    private bool bulletText = false;
    private bool collectFirstBullet = false;
    private bool hitPillarOnce = false;
    private bool shotOnce = false;
    private bool matchColor = false;
    private bool startShootingEnemy = false;


    // Start is called before the first frame update
    void Start()
    {
        conditionToTrue = ConditionToTrue;

        conditions.Add("movingTextDone", false);
        conditions.Add("moved", false);
        conditions.Add("hitPillar", false);
        conditions.Add("bullets", false);
        conditions.Add("runNearBullets", false);
        conditions.Add("clickLeftMouseText", false);
        conditions.Add("shootPillar", false);
        conditions.Add("matchColor", false);
        conditions.Add("endTut", false);


        explainationText.GetComponent<TMP_Text>().text = dialogues[0];
        currentText = Instantiate(explainationText, gameObject.transform);
        explainationText.GetComponent<TMP_Text>().text = dialogues[1];
        StartCoroutine(WaitThenDialogue(null, "movingTextDone", null, 1, 5));
        disableKey = false;

    }

    // Update is called once per frame
    void Update()
    {
        if(conditions["movingTextDone"] == true && (Input.GetKeyDown("w")|| Input.GetKeyDown("a")|| Input.GetKeyDown("s")|| Input.GetKeyDown("d")) )
        {
            Debug.Log("2 is running, movingTextdone = " + conditions["movingTextDone"]);
            StartCoroutine(WaitThenDialogue(null, "moved", null, 2,2));

            counter = countDownTime;
            pillarSpawn = true;
           
            conditions["movingTextDone"] = false;

            
        }

        if (counter > 0 && pillarSpawn)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                for(int i = 0; i < pillars.Count; i++)
                {
                    Instantiate(pillars[i], new Vector3(0+i, 0.5f, 0), Quaternion.identity);
                }
                pillarSpawn = false;

            }
        }



        if (conditions["hitPillar"] == true && !bulletText && !hitPillarOnce)
        {
            Debug.Log("3 is running, hitPillar = " + conditions["hitPillar"]);
            StartCoroutine(WaitThenDialogue(null, "bullets", null, 3, 2));
            hitPillarOnce = true;
            counter = countDownTime;
            bulletText = true;
        }


        if (counter > 0 && bulletText && !collectFirstBullet)
        {
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                StartCoroutine(WaitThenDialogue(null, "bullets", null, 4, 2));
                bulletText = false;
                collectFirstBullet = true;
                conditions["hitPillar"] = false;
            }
        }

        if (conditions["runNearBullets"] == true &&!clickLeftMouse &&!clickLeftMouseText)
        {
            Debug.Log("5 is running");
            StartCoroutine(WaitThenDialogue(null, "clickLeftMouseText", null, 5, 2));
            
            counter = countDownTime;
            clickLeftMouse = true;
            conditions["runNearBullets"] = false;

        }

        if (counter > 0 && clickLeftMouse)
        {
            //Debug.Log("Counter 6 is running");
            counter -= Time.deltaTime;
            if (counter <= 0)
            {
                StartCoroutine(WaitThenDialogue(null, "clickLeftMouseText", null, 6, 2));
                clickLeftMouse = false;
                clickLeftMouseText = true;
                conditions["runNearBullets"] = false;
                conditions["clickLeftMouseText"] = true;
            }    
        }

        if(conditions["clickLeftMouseText"] == true && conditions["shootPillar"])
        {
            //Debug.Log("7 is running, clickLeftMouseTextt = " + conditions["clickLeftMouseText"]);
            StartCoroutine(WaitThenDialogue(null, "shootPillar", null, 7, 2));
            Instantiate(enemyA, new Vector3(-2, 0, 0), Quaternion.identity);
            Instantiate(enemyB, new Vector3(-1, 0, 0), Quaternion.identity);
            conditions["clickLeftMouseText"] = false;
            counter = countDownTime;
            matchColor = true;
            //Debug.Log("7 is ending, clickLeftMouseTextt = " + conditions["clickLeftMouseText"]);
        }

        if(counter > 0 && matchColor)
        {
            counter -= Time.deltaTime;
            if(counter <= 0)
            {
                StartCoroutine(WaitThenDialogue(null, "matchColor", null, 8, 2));
                matchColor = false;
                startShootingEnemy = true;
            }
        }

        if(startShootingEnemy == true && GameObject.FindGameObjectsWithTag("Target").Length == 0)
        {
            StartCoroutine(WaitThenDialogue(null, "endTut", null, 9, 0));
            endScene.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
        }
    }

    IEnumerator WaitThenDialogue(NextDialogueDelegate conditionToTrue, string condition, bool? nextCondition, int dialogueIndex, float seconds)
    {
        yield return new WaitForSeconds(seconds);
        Destroy(currentText);
        explainationText.GetComponent<TMP_Text>().text = dialogues[dialogueIndex];
        currentText = Instantiate(explainationText, gameObject.transform);
        conditions[condition] = true;
        if(conditionToTrue != null)
        {
            yield return new WaitForSeconds(seconds);
            Destroy(currentText);
            explainationText.GetComponent<TMP_Text>().text = dialogues[dialogueIndex + 1];
            currentText = Instantiate(explainationText, gameObject.transform);
            ConditionToTrue(nextCondition);
        }
        
    }

    private void ConditionToTrue(bool? condition)
    {
        condition = true;
    }
}
