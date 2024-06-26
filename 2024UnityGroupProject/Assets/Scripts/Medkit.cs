using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] int healAmount;
    public static bool isInRange;
    [SerializeField] GameObject text;
    [SerializeField] TMP_Text pickUpText;

    private void Update()
    {
        text.transform.rotation = gameManager.instance.player.transform.rotation;
        
    }

    private void OnTriggerStay(Collider other)
    {
        if (playerController.didPickUp)
        {
            playerController.didPickUp = false;
            isInRange = false;
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        isInRange = true;
        text.SetActive(true);


        int tempPlayerHP = gameManager.instance.playerScript.GetHP();
        int HPOrig = gameManager.instance.playerScript.GetHPOrig();

        if(tempPlayerHP == HPOrig)
        {
            return;
        }


        medkitHeal heal = other.gameObject.GetComponent<medkitHeal>();

        if(heal != null)
        {
            heal.Heal(healAmount);
            Destroy(gameObject);
            isInRange = false;
        }
        

    }

    private void OnTriggerExit(Collider other)
    {
        isInRange = false;
        text.SetActive(false);
    }
}
