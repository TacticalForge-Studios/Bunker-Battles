using System.Collections;
using System.Collections.Generic;
//using Unity.PlasticSCM.Editor.WebApi;
using UnityEngine;
using UnityEngine.SceneManagement;

public class buttonFunctions : MonoBehaviour
{
    int currentLvl = 1;
    public static int HP;
    public static float stamina;
    public static int currentPlayerLvl;
    public static float playerArmor;
    public static int armorOrig;
    public static float maxXP;
    public static int xp;
    public static int currency;
    int currencyOrig;

    int pistolPrice = 50;
    int shotgunPrice = 70;
    int riflePrice = 100;
    int machineGunPrice = 80;
    int ammoPrice = 20;
    int healPrice = 20;

    private void Awake()
    {
        SetPlayerStats();
        currencyOrig = currency;
    }

    private void Update()
    {
        
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
            currency = 50;
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
        currency = currencyOrig;
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        gameManager.instance.stateUnPause();
    }

    public void Quit()
    {
        SceneManager.LoadScene(0);
        
//#if UNITY_EDITOR
//        UnityEditor.EditorApplication.isPlaying = false;
//#else
//            Application.Quit();
//#endif
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

    public void CloseStore()
    {
        gameManager.instance.storeScript.GunPurchased(6);
    }


    public void PurchasePistol()
    {

        if(currency < pistolPrice)
        {
            StartCoroutine(noMoney());
        }

        if(currency >= pistolPrice)
        {
            currency -= pistolPrice;
            gameManager.moneySaved = true;
            gameManager.instance.storeScript.UpdateStoreCurrencyText();
            gameManager.instance.UpdateCurrencyText(currency);
            gameManager.moneySaved = false;
            gameManager.instance.storeScript.GunPurchased(0);
        }
    }

    public void PurchaseShotgun()
    {
        if(currency < shotgunPrice)
        {
            StartCoroutine(noMoney());
        }

        if (currency >= shotgunPrice)
        {
            currency -= shotgunPrice;
            gameManager.moneySaved = true;
            gameManager.instance.storeScript.UpdateStoreCurrencyText();
            gameManager.instance.UpdateCurrencyText(currency);
            gameManager.moneySaved = false;
            gameManager.instance.storeScript.GunPurchased(1);

        }
    }

    public void PurchaseRifle()
    {
        if (currency < riflePrice)
        {
            StartCoroutine(noMoney());
        }

        if (currency >= riflePrice)
        {
            currency -= riflePrice;
            gameManager.moneySaved = true;
            gameManager.instance.storeScript.UpdateStoreCurrencyText();
            gameManager.instance.UpdateCurrencyText(currency);
            gameManager.moneySaved = false;
            gameManager.instance.storeScript.GunPurchased(2);

        }
    }

    public void PurchaseMachineGun()
    {
        if (currency < machineGunPrice)
        {
            StartCoroutine(noMoney());
        }

        if (currency >= machineGunPrice)
        {
            currency -= machineGunPrice;
            gameManager.moneySaved = true;
            gameManager.instance.storeScript.UpdateStoreCurrencyText();
            gameManager.instance.UpdateCurrencyText(currency);
            gameManager.moneySaved = false;
            gameManager.instance.storeScript.GunPurchased(3);

        }
    }

    public void PurchaseAmmo()
    {
        if (currency < ammoPrice)
        {
            StartCoroutine(noMoney());
        }

        if (currency >= ammoPrice)
        {
            currency -= ammoPrice;
            gameManager.moneySaved = true;
            gameManager.instance.storeScript.UpdateStoreCurrencyText();
            gameManager.instance.UpdateCurrencyText(currency);
            gameManager.moneySaved = false;
            gameManager.instance.storeScript.GunPurchased(4);

        }
    }

    public void PurchaseHeal()
    {
        if (currency < healPrice)
        {
            StartCoroutine(noMoney());
        }

        if (currency >= healPrice)
        {
            currency -= healPrice;
            gameManager.moneySaved = true;
            gameManager.instance.storeScript.UpdateStoreCurrencyText();
            gameManager.instance.UpdateCurrencyText(currency);
            gameManager.moneySaved = false;
            gameManager.instance.storeScript.GunPurchased(5);

        }
    }

    IEnumerator noMoney()
    {
        gameManager.instance.storeScript.noMoneyPopUp.SetActive(true);
        yield return new WaitForSeconds(2);
        gameManager.instance.storeScript.noMoneyPopUp.SetActive(false);
    }

}
