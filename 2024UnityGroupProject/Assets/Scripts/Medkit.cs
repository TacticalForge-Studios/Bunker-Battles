using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] int healAmount;



    private void OnTriggerEnter(Collider other)
    {
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
        }
        

    }
}
