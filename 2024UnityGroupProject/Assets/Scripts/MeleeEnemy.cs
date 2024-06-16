using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;

public class MeleeEnemy : MonoBehaviour, IDamage
{

    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform hitPos;
    [SerializeField] Transform headPos;

    [SerializeField] int HP;
    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;
    [SerializeField] GameObject EnemyUI;

    [SerializeField] float hitRate;

    public Image enemyHPBarBack;
    public Image enemyHPBar;
    public Image enemyName;

    bool isHitting;
    bool playerInRange;
    bool destChosen;
    bool isDead;

    Vector3 playerDir;
    Vector3 startingPos;

    int HPOrig;

    float angleToPlayer;
    float stoppingDistOrig;

    // Start is called before the first frame update
    void Start()
    {
        startingPos = transform.position;
        stoppingDistOrig = agent.stoppingDistance;
        HPOrig = HP;
        UpdateEnemyUI();
        //gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {

        float animSpeed = agent.velocity.normalized.magnitude;
        anim.SetFloat("Speed", Mathf.Lerp(anim.GetFloat("Speed"), animSpeed, Time.deltaTime * animSpeedTrans));

        if (playerInRange && !canSeePlayer())
        {
            StartCoroutine(roam());
        }
        else if (!playerInRange)
        {
            StartCoroutine(roam());
        }

        enemyHPBar.transform.rotation = gameManager.instance.player.transform.rotation;
        enemyHPBarBack.transform.rotation = gameManager.instance.player.transform.rotation;
        enemyName.transform.rotation = gameManager.instance.player.transform.rotation;
    }

    IEnumerator roam()
    {
        if(!destChosen && agent.remainingDistance < 0.05f)
        {
            destChosen = true;
            agent.stoppingDistance = 0;
            yield return new WaitForSeconds(roamTimer);

            Vector3 randPos = Random.insideUnitSphere * roamDist;
            randPos += startingPos;

            NavMeshHit hit;
            NavMesh.SamplePosition(randPos, out hit, roamDist, 1);
            agent.SetDestination(hit.position);
            destChosen = false;
        }
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - headPos.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);

        Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y + 1, playerDir.z));

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);
                

                if (!isHitting)
                {
                    StartCoroutine(melee());
                }
                if (agent.remainingDistance <= agent.stoppingDistance && !isDead)
                {
                    faceTarget();
                }
                return true;
            }
        }
        agent.stoppingDistance = 0;
        return false;
    }

    void faceTarget()
    {
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, 0, playerDir.z));
        transform.rotation = Quaternion.Lerp(transform.rotation, rot, Time.deltaTime * faceTargetSpeed);
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
        if (!isDead)
        {
            EnemyUI.SetActive(true);
            anim.SetTrigger("TakeDamage");
            UpdateEnemyUI();
            
        }


        if (HP <= 0)
        {
            anim.SetBool("isDead", true);
            if (!isDead)
            {
                gameManager.instance.playerScript.giveXP(60);
                gameManager.instance.UpdateCurrencyText(10);
                hitRate = 0;
                agent.velocity = Vector3.zero;
                agent.acceleration = 0;
                anim.SetTrigger("Death");
                EnemyUI.SetActive(false);
                isDead = true;
            }



        }
    }

    public void death()
    {
        Destroy(gameObject);
        gameManager.instance.UpdateGameGoal(-1);
    }

    //IEnumerator flashRed(Color color)
    //{


    //    if (model.material.color == Color.blue)
    //    {
    //        model.material.color = Color.red;

    //        yield return new WaitForSeconds(0.1f);

    //        model.material.color = Color.blue;
    //    }
    //    else if (model.material.color == Color.white)
    //    {
    //        model.material.color = Color.red;

    //        yield return new WaitForSeconds(0.1f);

    //        model.material.color = Color.white;
    //    }
    //    else if (model.material.color == Color.black)
    //    {
    //        model.material.color = Color.red;

    //        yield return new WaitForSeconds(0.1f);

    //        model.material.color = Color.black;
    //    }

    //}

    IEnumerator melee()
    {
        isHitting = true;
        anim.SetTrigger("Melee");
        yield return new WaitForSeconds(hitRate);
        
        isHitting = false;
    }

    void UpdateEnemyUI()
    {
        enemyHPBar.fillAmount = (float)HP / HPOrig;
    }
}
