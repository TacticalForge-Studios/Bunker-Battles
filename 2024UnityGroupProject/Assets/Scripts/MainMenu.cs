using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static bool isNewGame = false;
    public static bool isTutorial;

    private void Awake()
    {
        isNewGame = false;
    }
    public void PlayGame()
    {
        isNewGame = true;
        SceneManager.LoadScene("Level 1");
        
        
    }

    public void LoadTutorial()
    {
        isTutorial = true;
        SceneManager.LoadScene("Tutorial Level");
        buttonFunctions.currency = 500;
        
    }



    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Player Has Quit Game");
    }
}
