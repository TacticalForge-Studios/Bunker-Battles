using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;


public class EnemyAI : MonoBehaviour
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    

    [SerializeField] int HP;
    [SerializeField] int damageDone;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    bool isShooting;
    bool playerInRange;

    // Start is called before the first frame update
    void Start()
    {
        gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.transform.position);

            if (!isShooting)
            {
                StartCoroutine(shoot());
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
        }
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashRed());

        if(HP <= 0)
        {
            gameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed()
    {
        model.material.color = Color.red;

        yield return new WaitForSeconds(0.1f);

        model.material.color = model.material.color;
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }




}
