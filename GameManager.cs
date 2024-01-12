using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab.ClientModels;

public class GameManager : MonoBehaviour
{
    public GameObject player;
    private int playerScore;
    public int PlayerScore
    {
        get { return playerScore; }
    }
    public GameObject endGameScene;

    private PlayerData playerData;
    private NewLookScript lookScript;
    private NewMoveScript moveScript;
    private GunScript gunScript;
    private bool endGame;
    private GameObject[] targets;

    [SerializeField]
    private GameObject playfabManager;
    [SerializeField]
    public GameObject leaderboard;
    [SerializeField]
    public GameObject spawnManager;

    private void Start()
    {
        
        playerData = player.GetComponent<PlayerData>();
        lookScript = player.GetComponent<NewLookScript>();
        moveScript = player.GetComponent<NewMoveScript>();
        gunScript = GameObject.Find("Pistol 1").GetComponent<GunScript>();
        playfabManager = GameObject.Find("PlayFab Manager");
    }

    public void EndGame()
    {
        Debug.Log("End game scene");
        

        endGameScene.SetActive(true);
        lookScript.SetLookInactive();
        moveScript.SetMovementInactive();
        gunScript.DisableShooting();
        Cursor.lockState = CursorLockMode.None;

        spawnManager.GetComponent<SpawnScript>().RunSpawnSlider = false;

        targets = GameObject.FindGameObjectsWithTag("Target");
        foreach (GameObject target in targets)
        {
            TargetScript targetScript = target.GetComponent<TargetScript>();
            targetScript.Die();
        }

        playerScore = playerData.GetScore();
        //playfabManager.GetComponent<PlayFabManager>().SubmitScore(playerData.GetScore());
    }

    public void ShowLeaderboard(List<PlayerLeaderboardEntry> leaderboardEntries)
    {
        //playfabManager.GetComponent<PlayFabManager>().SubmitLeaderboard();

        endGameScene.SetActive(false);
        leaderboard.SetActive(true);

        //GameObject playFabManager = GameObject.Find("PlayFab Manager");
        //var leaderboardEntries = playFabManager.GetComponent<PlayFabManager>().LeaderboardEntries;
        //Debug.Log("Show Leaderboard: " + leaderboardEntries[0].DisplayName);

        leaderboard = GameObject.Find("Leaderboard");
        LeaderboardManager leaderboardManager = leaderboard.GetComponent<LeaderboardManager>();
        leaderboardManager.DisplayLeaderboard(leaderboardEntries);
        
    }

}
