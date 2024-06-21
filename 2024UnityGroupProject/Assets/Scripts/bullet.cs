using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class bullet : MonoBehaviour
{
    [SerializeField] Rigidbody rb;

    [SerializeField] int damage;
    [SerializeField] int speed;
    [SerializeField] int destroyTime;
    [SerializeField] float bulletGravity;
    [SerializeField] bool enableGravity;
    [SerializeField] bool isSniper;
    [SerializeField] bool isGeneral;
    Vector3 shotOffset;

    // Start is called before the first frame update
    void Start()
    {
        
        chooseOffset();
        rb.useGravity = enableGravity;
        Physics.gravity = new Vector3 (0, bulletGravity, 0);

        Vector3 playerDir = new Vector3(gameManager.instance.player.transform.position.x, gameManager.instance.player.transform.position.y, gameManager.instance.player.transform.position.z);

        if (transform.position.y < gameManager.instance.player.transform.position.y)
        {
            rb.velocity = (playerDir - transform.position + shotOffset).normalized * speed;
        }
        else if (transform.position.y > gameManager.instance.player.transform.position.y)
        {
            rb.velocity = (playerDir - transform.position + shotOffset).normalized * speed;
        }
        else
        {
            rb.velocity = transform.forward + shotOffset * speed;
        }

        Destroy(gameObject, destroyTime);

    }

    private void Update()
    {
        
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.isTrigger)
        {
            return;
        }

        IDamage friendlyFire = other.gameObject.GetComponent<EnemyAI>();

        if (friendlyFire != null)
        {
            return;

        }

        IDamage friendlyMelee = other.gameObject.GetComponent<MeleeEnemy>();

        if (friendlyMelee != null)
        {
            return;
        }

        IDamage dmg = other.gameObject.GetComponent<IDamage>();

        IDamage friendlyturret = other.gameObject.GetComponent<machineGunTurret>();

        if (friendlyturret != null)
        {
            return;
        }

        if (dmg != null)
        {
            dmg.takeDamage(damage);

        }

        Destroy(gameObject);
    }

    void chooseOffset()
    {
        if (isSniper)
        {
            shotOffset = new Vector3(Random.Range(-.9f, .9f), Random.Range(-.9f, .9f), Random.Range(-.9f, .9f));
        }
        else if (isGeneral)
        {
            shotOffset = new Vector3(Random.Range(-.8f, .8f), Random.Range(-.8f, .8f), Random.Range(-.8f, .8f));
        }
        else
        {
            shotOffset = new Vector3(Random.Range(-1f, 1f), Random.Range(-1f, 1f), Random.Range(-1f, 1f));
        }
        
        
    }

    public int getDamage()
    {
        return damage;
    }

    public void setDamage(int _damage)
    {
        damage = _damage;
    }
}
