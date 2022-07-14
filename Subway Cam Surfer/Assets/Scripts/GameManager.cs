using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class GameManager : MonoBehaviour
{
    int score;
    public static GameManager inst;

    public TextMeshProUGUI scoreText;
    //public Text scoreText;

    [SerializeField] PlayerMovement playerMovement;

    public void IncrementScore()
    {
        score++;
        scoreText.text = "Score: " + score;
        // Increase the player's speed
        playerMovement.fwdSpeed += playerMovement.speedIncreasePerPoint;
    }

    private void Awake()
    {
        inst = this;
    }

    private void Start()
    {

    }

    private void Update()
    {

    }

    public void ReplayGame() {

        SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);
       // SceneManager.LoadSceneAsync("SampleScene", LoadSceneMode.Additive);

    }
    public void MainMenu()
    {

        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex - 1);

    }
    public void GameQuit()
    {
        #if UNITY_EDITOR
            // Application.Quit() does not work in the editor so
            // UnityEditor.EditorApplication.isPlaying need to be set to false to end the game
            UnityEditor.EditorApplication.isPlaying = false;
        #else
             Application.Quit();
        #endif
        //Application.Quit();
    }

}
