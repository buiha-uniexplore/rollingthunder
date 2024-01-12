using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene : MonoBehaviour
{
    public void Replay()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void StartGame()
    {
        SceneManager.LoadScene("Gameplay");
    }

    public void ToTitle()
    {
        SceneManager.LoadScene("Title Scene");
    }

    public void ToTutorial()
    {
        SceneManager.LoadScene("Tutorial");
    }
}
