using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    int currentLvl = 0;

   public void Resume()
    {
        gameManager.instance.stateUnPause();
    }

    public void RespawnPlayer()
    {
        gameManager.instance.playerScript.spawnPlayer();
        gameManager.instance.stateUnPause();
    }

    public void Restart()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnPause();
    }

    public void Quit()
    {
    #if UNITY_EDITOR
            UnityEditor.EditorApplication.isPlaying = false;
    #else
            Application.Quit();
    #endif
    }

    public void IncreaseHealth()
    {
        int currentHealth = gameManager.instance.playerScript.GetHPOrig();
        currentHealth += 10;
        gameManager.instance.playerScript.setHP(currentHealth);
        gameManager.instance.stateUnPause();
        
    }

    public void IncreaseStamina()
    {
        float currentStamina = gameManager.instance.playerScript.getStaminaMax();
        currentStamina += 10.0f;
        gameManager.instance.playerScript.setStamina(currentStamina);
        gameManager.instance.stateUnPause();
    }

    public void NextLevel()
    {
        gameManager.instance.statePause();
        currentLvl++;
        SceneManager.LoadScene(currentLvl);
        gameManager.instance.stateUnPause();
    }
}
