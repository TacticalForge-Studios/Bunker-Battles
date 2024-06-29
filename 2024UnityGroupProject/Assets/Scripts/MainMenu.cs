using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    [SerializeField] string ID = "Bunker Battles-1";

    public static bool isNewGame = false;
    public static bool isTutorial;
    public static bool isLoaded = false;

    private void Awake()
    {
        isNewGame = false;

    }
    public void PlayGame()
    {
        isNewGame = true;
        SaveLoadManager.instance.ClearSave();
        SceneManager.LoadScene("Level 1");

        if (buttonFunctions.gunList.Count > 0)
        {
            foreach (gunStats gun in buttonFunctions.gunList)
            {
                buttonFunctions.gunList.Remove(gun);
            }
        }




    }

    public void LoadTutorial()
    {
        isTutorial = true;
        SceneManager.LoadScene("Tutorial Level");
        buttonFunctions.currency = 500;

    }

    public void Continue()
    {
        // are we loading from a save file
        if (SaveLoadManager.instance.savedState != null)
        {
            isLoaded = true;
            var playerState = SaveLoadManager.instance.savedState.playerState;
            if (playerState.ID == ID)
            {
                foreach (var entry in playerState.entries)
                {
                    PlayerStats playerHP = entry.playerHP;
                    PlayerStats playerArmor = entry.playerArmor;
                    Vector3 location = new Vector3(entry.location.Item1, entry.location.Item2, entry.location.Item3);


                    if (playerState.level == "Level 1")
                    {
                        SceneManager.LoadScene("Level 1");
                        gameManager.instance.playerScript.setHP((int)playerHP);
                        gameManager.instance.playerScript.setArmor((int)playerArmor);
                        gameManager.instance.playerSpawnPos.transform.position = location;
                        gameManager.instance.playerScript.updatePlayerUI();

                    }
                    else if (playerState.level == "Level 2")
                    {
                        SceneManager.LoadScene("Level 2");
                        gameManager.instance.playerScript.setHP((int)playerHP);
                        gameManager.instance.playerScript.setArmor((int)playerArmor);
                        gameManager.instance.playerSpawnPos.transform.position = location;
                        gameManager.instance.playerScript.updatePlayerUI();

                    }
                    else if (playerState.level == "Level 3")
                    {
                        SceneManager.LoadScene("Level 3");
                        gameManager.instance.playerScript.setHP((int)playerHP);
                        gameManager.instance.playerScript.setArmor((int)playerArmor);
                        gameManager.instance.playerSpawnPos.transform.position = location;
                        gameManager.instance.playerScript.updatePlayerUI();
                    }





                }
                
                return;
            }



        }
    }



    public void QuitGame()
    {
        Application.Quit();
        Debug.Log("Player Has Quit Game");
    }
}
