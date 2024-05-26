using System.Collections;
using System.Collections.Generic;
using System.Transactions;
using UnityEngine;

public class meleeHit : MonoBehaviour

{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] bool hit;
    
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

        IDamage friendlyFire1 = other.gameObject.GetComponent<EnemyAI>();
        if (friendlyFire1 != null)
        {
            return;
        }

        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        if (dmg != null )
        {
            hit = !hit;

            if(hit == true)
            {
                StartCoroutine(dealDamage(dmg));
                
            }
            
        }
    }
    
    IEnumerator dealDamage(IDamage dmg)
    {
        if(hit == true)
        {
            
            dmg.takeDamage(damage);
            yield return new WaitForSeconds(5);
            hit = false;
        }
    }
        

        
    
}
