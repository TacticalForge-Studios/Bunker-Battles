using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.UI;


public class EnemyAI : MonoBehaviour, IDamage
{
    [SerializeField] Animator anim;
    [SerializeField] NavMeshAgent agent;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;
    [SerializeField] Transform itemSpawnPos;
    

    [SerializeField] int HP;
    [SerializeField] int viewAngle;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int roamDist;
    [SerializeField] int roamTimer;
    [SerializeField] GameObject EnemyUI;
    [SerializeField] GameObject ItemToSpawn;

    [SerializeField] GameObject bullet;
    [SerializeField] float shootRate;


    public Image enemyHPBarBack;
    public Image enemyHPBar;
    public Image enemyName;
    public Image sniperSkull;

    bool isShooting;
    bool playerInRange;
    bool destChosen;
    bool isDead = false;

    Vector3 playerDir;
    Vector3 startingPos;

    int HPOrig;
    int dropChance;

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
        if(sniperSkull != null)
        {
            sniperSkull.transform.rotation = gameManager.instance.player.transform.rotation;
        }
        
        
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

        
        //Debug.Log(angleToPlayer);
        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y + 1, playerDir.z));

        RaycastHit hit;
        if(Physics.Raycast(headPos.position, playerDir, out hit))
        {
            if (hit.collider.CompareTag("Player") && angleToPlayer <= viewAngle)
            {
                agent.stoppingDistance = stoppingDistOrig;
                agent.SetDestination(gameManager.instance.player.transform.position);

                if (!isShooting && HP >= 1)
                {
                    StartCoroutine(shoot());
                }
                if (agent.remainingDistance <= agent.stoppingDistance)
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
        Quaternion rot = Quaternion.LookRotation(new Vector3(playerDir.x, playerDir.y, playerDir.z));
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
            agent.stoppingDistance = 0;
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
        

        if(HP <= 0)
        {
            dropChance = Random.Range(0, 100);

            if(dropChance >= 50)
            {
                Instantiate(ItemToSpawn, itemSpawnPos.position, transform.rotation);
            }
            
            if (!isDead)
            {
                gameManager.instance.playerScript.giveXP(60);
                shootRate = 0;
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
    //    else if(model.material.color == Color.white)
    //    {
    //        model.material.color = Color.red;

    //        yield return new WaitForSeconds(0.1f);

    //        model.material.color = Color.white;
    //    }
    //    else if(model.material.color == Color.black)
    //    {
    //        model.material.color = Color.red;

    //        yield return new WaitForSeconds(0.1f);

    //        model.material.color = Color.black;
    //    }
        
    //}

    IEnumerator shoot()
    {
        isShooting = true;
        anim.SetTrigger("Shoot");
        
        yield return new WaitForSeconds(shootRate);

        isShooting = false;
        
    }

    public void createBullet()
    {
        Instantiate(bullet, shootPos.position, transform.rotation);
    }

    public void UpdateEnemyUI()
    {
        enemyHPBar.fillAmount = (float)HP / HPOrig;
        
    }

    public int GetHP()
    {
        return HP;
    }

}
