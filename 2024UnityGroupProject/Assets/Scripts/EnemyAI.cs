using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyAI : MonoBehaviour, IDamage
{

    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    

    [SerializeField] int HP;
    [SerializeField] int damageDone;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;

    public Image enemyHPBar;

    bool isShooting;
    bool playerInRange;

    int HPOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        UpdateEnemyUI();
        gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

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
        UpdateEnemyUI();
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashRed(model.material.color));

        if(HP <= 0)
        {
            gameManager.instance.UpdateGameGoal(-1);
            Destroy(gameObject);
        }
    }

    IEnumerator flashRed(Color color)
    {
        

        if (model.material.color == Color.blue)
        {
            model.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            model.material.color = Color.blue;
        }
        else if(model.material.color == Color.white)
        {
            model.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            model.material.color = Color.white;
        }
        else if(model.material.color == Color.black)
        {
            model.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            model.material.color = Color.black;
        }
        
    }

    IEnumerator shoot()
    {
        isShooting = true;
        Instantiate(bullet, shootPos.position, transform.rotation);
        yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }

    public void UpdateEnemyUI()
    {
        enemyHPBar.fillAmount = (float)HP / HPOrig;
    }




}
