using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ammoCan : MonoBehaviour
{
    [SerializeField] int resupplyAmount;

    private void OnTriggerEnter(Collider other)
    {
        int tempTotalAmmoAmount = gameManager.instance.playerScript.getTotalAmmo();
        int ammoMax = gameManager.instance.playerScript.getAmmoMax();

        if(tempTotalAmmoAmount == ammoMax)
        {
            return;
        }

        ammoResupply toResupply = other.gameObject.GetComponent<ammoResupply>();

        if(toResupply != null)
        { 
            toResupply.Resupply(resupplyAmount);
            Destroy(gameObject);
        }


    }



}
