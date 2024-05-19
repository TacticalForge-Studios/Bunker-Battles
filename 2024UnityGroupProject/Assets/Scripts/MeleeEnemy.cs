using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemy : MonoBehaviour, IDamage
{
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform hitPos;

    [SerializeField] int HP;
    [SerializeField] int damageDone;

    [SerializeField] float hitRate;

    public Image enemyHPBarBack;
    public Image enemyHPBar;

    bool isHitting;
    bool playerInRange;

    int HPOrig;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        UpadateEnemyUI();
        gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange)
        {
            agent.SetDestination(gameManager.instance.player.transform.position);

            if (!isHitting)
            {
                StartCoroutine(melee());
            }
        }
        enemyHPBar.transform.rotation = gameManager.instance.player.transform.rotation;
        enemyHPBarBack.transform.rotation = gameManager.instance.player.transform.rotation;
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
        UpadateEnemyUI();
        agent.SetDestination(gameManager.instance.player.transform.position);
        StartCoroutine(flashRed(model.material.color));

        if (HP <= 0)
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
        else if (model.material.color == Color.white)
        {
            model.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            model.material.color = Color.white;
        }
        else if (model.material.color == Color.black)
        {
            model.material.color = Color.red;

            yield return new WaitForSeconds(0.1f);

            model.material.color = Color.black;
        }

    }

    IEnumerator melee()
    {
        isHitting = true;
        yield return new WaitForSeconds(hitRate);
        isHitting = false;
    }

    void UpadateEnemyUI()
    {
        enemyHPBar.fillAmount = (float)HP / HPOrig;
    }
}
