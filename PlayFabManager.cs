using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using PlayFab;
using PlayFab.ClientModels;
using TMPro;

public class PlayFabManager : MonoBehaviour
{
    
    private int scoreToSubmit;

    [SerializeField]
    private Slider leaderboardSlider;
    
    [SerializeField]
    private List<PlayerLeaderboardEntry> leaderboardEntries;
    public List<PlayerLeaderboardEntry> LeaderboardEntries
    {
        get { return leaderboardEntries; }
    }
    [SerializeField]
    //private GameObject leaderboard;
    private GameObject displayNameInputField;
    private GameObject spawnManager;
    private GameObject gameManager;
    // Start is called before the first frame update
    void Start()
    {
        
        //Login();
    }

    public void Login()
    {
        var request = new LoginWithCustomIDRequest
        {
            CustomId = System.Guid.NewGuid().ToString(),
            CreateAccount = true
        };

        PlayFabClientAPI.LoginWithCustomID(request, OnLoginSucess, OnLoginError);
    }

    void OnLoginSucess(LoginResult result)
    {
        Debug.Log("Sucessfully created customID");
        PlayerPrefs.SetString("Custom ID", result.EntityToken.Entity.Id);
    }

    void OnLoginError(PlayFabError error)
    {
        Debug.LogWarning("Something went wrong with creating customID");
        Debug.LogError("Here are the error information");
        Debug.LogError(error.GenerateErrorReport());
    }

     public void SubmitDisplayName()
    {
        displayNameInputField = GameObject.Find("Display Name Input");
        if (displayNameInputField == null)
        {
            Debug.Log("Did not find display name input field");
        }

        string name = displayNameInputField.GetComponent<TMP_InputField>().text;
        if (name.Length < 3)
        {
            name = name + "  ";
        }

        var request = new UpdateUserTitleDisplayNameRequest
        {
            DisplayName = name
        };

        PlayFabClientAPI.UpdateUserTitleDisplayName(request, OnDisplayNameUpdate, OnDisplayNameUpdateError);
    }

    void OnDisplayNameUpdate(UpdateUserTitleDisplayNameResult result)
    {
        Debug.Log("Update Display Name success");
        SubmitLeaderboard();
    }

    void OnDisplayNameUpdateError(PlayFabError error)
    {
        Debug.Log("Update Display Name error");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SubmitScore(int score)
    {
        scoreToSubmit = score;
        Debug.Log("PFM Updated scoreToSubmit:" + scoreToSubmit.ToString());
    }

    public void SubmitLeaderboard()
    {
        gameManager = GameObject.Find("Game Manager");
        Debug.Log("PFM Submit Leaderboard, scoreToSubmit: " + gameManager.GetComponent<GameManager>().PlayerScore.ToString());

        //SubmitDisplayName(name);

        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate> {
                new StatisticUpdate
                {
                    StatisticName = "Leaderboard",
                    Value = gameManager.GetComponent<GameManager>().PlayerScore
                }
            }

        };

        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnLeaderboardUpdateError);

        spawnManager = GameObject.Find("SpawnManager");
        if (spawnManager != null)
        {
            spawnManager.GetComponent<SpawnScript>().RunSpawnSlider = false;
        }

        
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Succesfully updated leaderboard");

        gameManager = GameObject.Find("Game Manager");

        IEnumerator coroutine;
        coroutine = WaitThenGetLeaderBoard();
        gameManager.GetComponent<GameManager>().StartCoroutine(coroutine);
        //GetLeaderboardStatistics();
    }

    IEnumerator WaitThenGetLeaderBoard()
    {
        leaderboardSlider = GameObject.Find("Leaderboard Slider").GetComponent<Slider>();

        Debug.Log("Waiting");
        for(int i = 0; i < 100; i++)
        {
            yield return new WaitForSeconds(5f/100);
            leaderboardSlider.value += 1f / 100;
        }
        
        GetLeaderboardStatistics();
    }

    void OnLeaderboardUpdateError(PlayFabError error)
    {
        Debug.Log("Error updating leaderboard: ");
        Debug.Log(error.GenerateErrorReport());
    }

    public void GetLeaderboardStatistics()
    {
        Debug.Log("Getting Leaderboard Statistics");
        var request = new GetLeaderboardRequest()
        {
            StatisticName = "Leaderboard",
            MaxResultsCount = 5,
            StartPosition = 0,
        };

        PlayFabClientAPI.GetLeaderboard(request, OnGetLeaderboardSuccess, OnGetLeaderboardError);
    }

    void OnGetLeaderboardSuccess(GetLeaderboardResult result)
    {
        Debug.Log("Successfully got leaderboard");
        leaderboardEntries = result.Leaderboard;
        Debug.Log("PFM: Display Name: " + leaderboardEntries[0].DisplayName);
        Debug.Log("PFM: Stat value: " + leaderboardEntries[0].StatValue.ToString()) ;

        gameManager = GameObject.Find("Game Manager");
        var gameManagerScript = gameManager.GetComponent<GameManager>(); 
        gameManagerScript.ShowLeaderboard(result.Leaderboard);
    }

    void OnGetLeaderboardError(PlayFabError error)
    {
        Debug.Log("There was an error getting leaderboard");
        Debug.Log(error.GenerateErrorReport());
    }
}
