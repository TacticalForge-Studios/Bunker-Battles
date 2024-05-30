using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Unity.VisualScripting;

public class gameManager : MonoBehaviour
{
    public static gameManager instance;

    [SerializeField] GameObject menuActive;
    [SerializeField] GameObject menuPause;
    [SerializeField] GameObject menuWin;
    [SerializeField] GameObject menuLose;


    public Image playerHPBar;

    public Image playerStaminaBar;

    public Image playerXPBar;

    public GameObject player;
    

    [SerializeField] TMP_Text enemyCountText;
    public TMP_Text ammoCurrText;
    public TMP_Text ammoMaxText;

    public GameObject playerFlashDamage;

    
    public playerController playerScript;
   
    public bool isPaused;
    int enemyCount;

    // Start is called before the first frame update
    void Awake()
    {
        instance = this;
        player = GameObject.FindWithTag("Player");
        playerScript = player.GetComponent<playerController>();
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
                stateUnPause();
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
        menuActive.SetActive(isPaused);
        menuActive = null;
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
}
