using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using PlayFab;
using PlayFab.ClientModels;

public class LeaderboardManager : MonoBehaviour
{
    [SerializeField]
    private GameObject canvas;
    [SerializeField]
    private GameObject leaderboard;
    [SerializeField]
    private GameObject entryPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void DisplayLeaderboard(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        canvas = GameObject.Find("Canvas");
        leaderboard = GameObject.Find("Leaderboard");

        for(int i = 0; i < leaderboardEntries.Count; i++)
        {
            Debug.Log(entryPrefab.GetComponent<TMP_Text>().text + (i + 1) + ". " + leaderboardEntries[i].DisplayName + " " + leaderboardEntries[i].StatValue.ToString());
            entryPrefab.GetComponent<TMP_Text>().text = entryPrefab.GetComponent<TMP_Text>().text + (i+1) + ". " + leaderboardEntries[i].DisplayName + " " + leaderboardEntries[i].StatValue.ToString() + "\n";
        }
        
        var obj = Instantiate(leaderboard, leaderboard.transform);
        obj.transform.localPosition = new Vector3(0f, 0f, 0f);
    }
}
