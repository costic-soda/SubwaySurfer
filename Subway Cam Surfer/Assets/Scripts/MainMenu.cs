using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    //void start()
    //{
    //    SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 2);

    //}
    //private static bool created = false;

    //void Awake()
    //{
    //    if (!created)
    //    {
    //        DontDestroyOnLoad(this.gameObject);
    //        created = true;
    //        Debug.Log("Awake: " + this.gameObject);
    //    }
    //}

    public void PlayGame()
    {
       
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);

    }

  

    public void QuitGame()
    {
        Application.Quit();

    }
}
