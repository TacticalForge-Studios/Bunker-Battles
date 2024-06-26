using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class store : MonoBehaviour
{
    [Header("Store UI")]
    [SerializeField] public GameObject storeUI;

    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject textMessagePopUp;
    [SerializeField] public GameObject noMoneyPopUp;
    [SerializeField] public GameObject ownedPopUp;

    [Header("Purchasables")]
    [SerializeField] public gunStats Pistol;
    [SerializeField] public gunStats Rifle;
    [SerializeField] public gunStats Shotgun;
    [SerializeField] public gunStats machineGun;

    [SerializeField] TMP_Text MoneyText;
    public TMP_Text moneyTextCount;
    int ammo;
    int health;

    bool isOpened;
    static public bool isInRange;
    

    private void Update()
    {
        textMessagePopUp.transform.rotation = gameManager.instance.player.transform.rotation;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textMessagePopUp.SetActive(true);
            isInRange = true;
           
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textMessagePopUp.SetActive(false);
            isInRange = false;
        }
    }

    public void OpenStore()
    {
        gameManager.instance.menuActive = gameManager.instance.menuPause;
        gameManager.instance.storeUI = true;
        UpdateStoreCurrencyText();
        anim.SetBool("isOpened", true);
        //gameManager.instance.statePause();
        storeUI.SetActive(true);
        gameManager.instance.statePause();

        
    }

    public void UpdateStoreCurrencyText()
    {
        moneyTextCount.text = buttonFunctions.currency.ToString("F0");
    }

    public void GunPurchased(int number)
    {
        switch(number)
        {
            case 0:
                gameManager.instance.playerScript.pickUpGun(Pistol);
                anim.SetBool("isOpened", false);
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;
            case 1:
                gameManager.instance.playerScript.pickUpGun(Shotgun);
                anim.SetBool("isOpened", false);
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;
            case 2:
                gameManager.instance.playerScript.pickUpGun(Rifle);
                anim.SetBool("isOpened", false);
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;
            case 3:
                gameManager.instance.playerScript.pickUpGun(machineGun);
                anim.SetBool("isOpened", false);
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;
            case 4:
                int amountOfGuns = gameManager.instance.playerScript.gunList.Count;
                for(int i = 0; i < amountOfGuns; i++)
                {
                    gameManager.instance.playerScript.gunList[i].totalAmmoLeft = gameManager.instance.playerScript.gunList[i].ammoMax;
                }
                anim.SetBool("isOpened", false); 
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;
            case 5:
                gameManager.instance.playerScript.setCurrentHealth(gameManager.instance.playerScript.GetHPOrig());
                anim.SetBool("isOpened", false);
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;
            case 6:
                anim.SetBool("isOpened", false);
                storeUI.SetActive(false);
                gameManager.instance.stateUnPause();
                break;

        }
    }

}
