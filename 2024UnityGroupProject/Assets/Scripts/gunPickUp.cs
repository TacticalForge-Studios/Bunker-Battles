using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class gunPickUp : MonoBehaviour
{
    [SerializeField] gunStats gun;
    // Start is called before the first frame update
    void Start()
    {
        gun.ammoCurrent = gun.magCapacity;
        gun.totalAmmoLeft = (gun.ammoMax -  gun.ammoCurrent);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            gameManager.instance.playerScript.pickUpGun(gun);
            Destroy(gameObject);
        }
    }
}
