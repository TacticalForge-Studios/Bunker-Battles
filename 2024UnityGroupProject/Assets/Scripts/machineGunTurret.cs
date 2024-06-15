using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class machineGunTurret : MonoBehaviour, IDamage
{
    [Header("Animators and Positions")]
    [SerializeField] Animator anim;
    [SerializeField] Renderer model;
    [SerializeField] Transform shootPos;
    [SerializeField] Transform headPos;

    [Header("Sats")]
    [SerializeField] int HP;
    [SerializeField] int animSpeedTrans;
    [SerializeField] int faceTargetSpeed;
    [SerializeField] float shootRate;
    [SerializeField] int viewAngle;
    [SerializeField] GameObject bullet;

    [Header("UI")]
    [SerializeField] GameObject TurretUI;
    public Image turrentHPBarBack;
    public Image turrentHPBar;

    bool isShooting;
    bool playerInRange;
    bool isDead = false;

    Vector3 playerDir;

    int HPOrig;
    float angleToPlayer;





    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        UpdateTurretUI();
        gameManager.instance.UpdateGameGoal(1);
    }

    // Update is called once per frame
    void Update()
    {
        turrentHPBar.transform.rotation = gameManager.instance.player.transform.rotation;
        turrentHPBarBack.transform.rotation = gameManager.instance.player.transform.rotation;
        canSeePlayer();
    }

    bool canSeePlayer()
    {
        playerDir = gameManager.instance.player.transform.position - transform.position;
        angleToPlayer = Vector3.Angle(new Vector3(playerDir.x, playerDir.y + 1, playerDir.z), transform.forward);

        Debug.DrawRay(headPos.position, new Vector3(playerDir.x, playerDir.y, playerDir.z + 1));
        Debug.Log(angleToPlayer);
        RaycastHit hit;
        if(Physics.Raycast(shootPos.position, playerDir, out hit) && angleToPlayer <= viewAngle)
        {
            if(hit.collider.CompareTag("Player") && playerInRange)
            {
                
                if(!isShooting && HP >= 1 && angleToPlayer <= 90)
                {
                    anim.SetBool("isShooting", true);
                    StartCoroutine(shoot());
                }
                
                
                if (isDead == false)
                {
                    //faceTarget();
                }
                return true;
            }
            
            
        }
        anim.SetBool("isShooting", false);
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
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;
        //anim.SetBool("isShooting", true);

        Instantiate(bullet, shootPos.position, transform.rotation);

        yield return new WaitForSeconds(shootRate);

        isShooting = false;
        //anim.SetBool("isShooting", false);
    }

   

    public void takeDamage(int amount)
    {
        HP -= amount;
        UpdateTurretUI();

        if (!isDead)
        {
            TurretUI.SetActive(true);
            UpdateTurretUI();
        }

        if(HP <= 0)
        {
            anim.SetBool("isDead", true);

            if (!isDead)
            {
                gameManager.instance.playerScript.giveXP(50);
                gameManager.instance.UpdateCurrencyText(10);
                shootRate = 0;

                anim.SetTrigger("Death");
                TurretUI.SetActive(false);
                isDead = true;
            }
        }

    }

    private void death()
    {
        Destroy(gameObject);
        gameManager.instance.UpdateGameGoal(-1);
    }

    public void UpdateTurretUI()
    {
        turrentHPBar.fillAmount = (float)HP / HPOrig;
    }
}
