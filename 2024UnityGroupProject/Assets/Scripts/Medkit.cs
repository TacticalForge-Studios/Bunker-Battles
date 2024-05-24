using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Medkit : MonoBehaviour
{
    [SerializeField] int healAmount;


    private void OnTriggerEnter(Collider other)
    {
        medkitHeal heal = other.gameObject.GetComponent<medkitHeal>();

        if(heal != null)
        {
            heal.Heal(healAmount);
            
        }
        Destroy(gameObject);

    }
}
