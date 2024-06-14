using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    int currentLvl = 0;
    public static int HP;
    public static float stamina;
    public static int currentPlayerLvl;
    public static float playerArmor;
    public static int armorOrig;
    public static float maxXP;
    public static int xp;
    public static int currency;

    private void Awake()
    {
        SetPlayerStats();
        
    }

    private void SetPlayerStats()
    {
        if(HP == 0)
        {
            HP = 20;
            stamina = 100;
            currentPlayerLvl = 1;
            armorOrig = 10;
            playerArmor = armorOrig;
            maxXP = 100;
        }
        playerArmor = armorOrig;
        
    }

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
        //int currentHealth = gameManager.instance.playerScript.GetHPOrig();
        HP += 10;
        gameManager.instance.playerScript.setHP(HP);
        gameManager.instance.stateUnPause();
        
    }

    public void IncreaseStamina()
    {
        //float currentStamina = gameManager.instance.playerScript.getStaminaMax();
        //currentStamina += 10.0f;
        stamina += 10;
        gameManager.instance.playerScript.setStamina(stamina);
        gameManager.instance.stateUnPause();
    }

    public void IncreaseArmor()
    {
        armorOrig += 5;
        gameManager.instance.playerScript.setArmor(armorOrig);
        gameManager.instance.stateUnPause();
    }

    public void NextLevel()
    { 
        gameManager.instance.statePause();
        //int hp = gameManager.instance.playerScript.GetHPOrig();
        //float stamina = gameManager.instance.playerScript.getStaminaMax();
        //int lvl = gameManager.instance.playerScript.getLevel();
        currentLvl++;
        SceneManager.LoadScene(currentLvl);
        gameManager.instance.stateUnPause();
        gameManager.instance.playerScript.setHP(HP);
        gameManager.instance.playerScript.setStamina(stamina);
        gameManager.instance.playerScript.setLvl(currentPlayerLvl);
        gameManager.instance.playerScript.setXp(xp);
        gameManager.instance.playerScript.setXPMax(maxXP);
        

    }

}
