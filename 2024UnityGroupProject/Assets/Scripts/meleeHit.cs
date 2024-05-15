using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class meleeHit : MonoBehaviour

{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
            return;

        IDamage friendlyFire = other.gameObject.GetComponent<MeleeEnemy>();
        if (friendlyFire != null)
        {
            return;

        }

        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        if (dmg != null )
        {
            dmg.takeDamage(damage);
        }
    }
    
        

        
    
}
