using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class store : MonoBehaviour
{
    [Header("Store UI")]
    [SerializeField] GameObject storeUI;

    [Header("Animation")]
    [SerializeField] Animator anim;
    [SerializeField] GameObject textMessagePopUp;

    [Header("Purchasables")]
    [SerializeField] GameObject Pistol;
    [SerializeField] GameObject Rifle;
    [SerializeField] GameObject Shotgun;
    [SerializeField] GameObject machineGun;
    [SerializeField] int ammo;
    [SerializeField] int health;

    bool isOpened;
    

    private void Update()
    {
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textMessagePopUp.SetActive(true);
           
        }
        
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            textMessagePopUp.SetActive(false);
            
        }
    }

    public void OpenStore()
    {
        storeUI.SetActive(true);

        
    }

}
