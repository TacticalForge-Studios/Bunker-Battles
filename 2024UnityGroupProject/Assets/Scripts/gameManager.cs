using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] public GameObject menuActive;
    [SerializeField] public GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;
    [SerializeField] GameObject menuLevelUp;
    public bool storeUI;
    [SerializeField] GameObject menuSaveMenu;


    public Image playerHPBar;

    public Image playerArmorBar;

    public GameObject checkpointPopUp;

    public Image playerStaminaBar;

    public Image playerXPBar;

    public GameObject player;

    public GameObject store;
    

    [SerializeField] TMP_Text enemyCountText;
    [SerializeField] TMP_Text currencyCountText;
    public TMP_Text ammoCurrText;
    public TMP_Text ammoMaxText;
    

    public GameObject playerSpawnPos;
    public GameObject playerFlashDamage;

    public playerController playerScript;
    public store storeScript;

    public bool isPaused = false;
    public static bool moneySaved = false;
    int enemyCount;
    public int currency;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        store = GameObject.FindWithTag("Store");
        storeScript = store.GetComponent<store>();
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
        playerSpawnPos = GameObject.FindWithTag("Player Spawn Pos");
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetButtonDown("Cancel"))
        {
            if (menuActive == null)
            {
                statePause();
                menuActive = menuPause;
                menuActive.SetActive(isPaused);
            }
            else if (menuActive == menuPause)
            {
                if (storeUI)
                {
                    
                    storeUI = false;
                    storeScript.storeUI.SetActive(false);
                    stateUnPause();
                }
                else
                {
                    stateUnPause();
                }
                
            }
        }
    }

    public void statePause()
    {
        isPaused = !isPaused;
        Time.timeScale = 0;
        Cursor.visible = true;
        Cursor.lockState = CursorLockMode.Confined;
    }

    public void stateUnPause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        if(menuActive != null)
        {
            menuActive.SetActive(isPaused);
            menuActive = null;
        }
        
        
    }
    
    public void menuUnpause()
    {
        isPaused = !isPaused;
        Time.timeScale = 1;
        Cursor.visible = true;
        Cursor.lockState= CursorLockMode.Confined;
        if(menuActive != null)
        {
            menuActive.SetActive(isPaused);
            menuActive = null;
        }
    }

    public void UpdateGameGoal(int amount)
    {
        enemyCount += amount;
        enemyCountText.text = enemyCount.ToString("F0");

        if(enemyCount <= 0)
        {
            statePause();
            menuActive = menuWin;
            menuActive.SetActive(isPaused);
        }
    }

    public void YouLose()
    {
        statePause();
        menuActive = menuLose;
        menuActive.SetActive(isPaused);
    }

    public void LevelUp(int lvl)
    {
        statePause();
        menuActive = menuLevelUp;
        menuActive.SetActive(menuLevelUp);
    }

    public void UpdateCurrencyText(int amount)
    {
        if (!moneySaved)
        {
            buttonFunctions.currency += amount;
        }
        
        
        

        if (buttonFunctions.currency < 0)
        {
            buttonFunctions.currency = 0;
        }

        currencyCountText.text = buttonFunctions.currency.ToString("F0");
        
        
    }
}
