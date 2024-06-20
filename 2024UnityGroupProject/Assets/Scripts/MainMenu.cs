using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public static bool isNewGame;
   public void PlayGame()
    {
        isNewGame = true;
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        
        
    }

    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Player Has Quit Game");
    }
}
