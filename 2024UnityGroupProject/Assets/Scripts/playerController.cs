using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour, IDamage, medkitHeal
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject gunModel;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject muzzleFlash;

    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audJump;
    [Range (0,1)] [SerializeField] float audJumpVol;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    public ParticleSystem hitEffect;
    public ParticleSystem hitEffectBlood;
    

    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;
    int maxStamina = 75;
    int currentStamina;
    int staminaMod;
    int staminaRegen;
    int selectedGun;

    bool isShooting;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;

        currentStamina = maxStamina;
        updatePlayerUI();

        spawnPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused) 
        { 
           Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
           gunSelect();
           movement();
           updatePlayerUI();
        }
    }

    void movement()
    {
        sprint();
        
        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurrent > 0 && !isShooting)
        {
            StartCoroutine(Shoot());
        }

        if (Input.GetButtonDown("Reload"))
        {
            gunList[selectedGun].ammoCurrent = gunList[selectedGun].ammoMax;
            gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");
        }

        if (controller.isGrounded)
        {
            jumpCount = 0;
        }

        moveDir = (Input.GetAxis("Horizontal") * transform.right) + (Input.GetAxis("Vertical") * transform.forward);
        controller.Move(moveDir * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            aud.PlayOneShot(audJump[Random.Range(0, audJump.Length)], audJumpVol);
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y -= gravity * Time.deltaTime;
        controller.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed *= sprintMod;

           // while (currentStamina >= 0)
            //{
               //if (Input.GetButtonUp("Sprint"))
               //{
               //     break;
               //}
                currentStamina -= staminaMod;
                updatePlayerUI();
            //}
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;

            //while(true)
            //{
                currentStamina += staminaRegen;

                if(currentStamina >= maxStamina)
                {
                    currentStamina = maxStamina;
                   // break;
                }
                updatePlayerUI();
            //}
        }
    }

    IEnumerator Shoot()
    {
        isShooting = true;

        

        aud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootVolume);

        gunList[selectedGun].ammoCurrent--;
        gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");

        StartCoroutine(flashMuzzle());

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, shootDist))
        {
            //Debug.Log(hit.transform.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                dmg.takeDamage(shootDamage);
                Instantiate(hitEffectBlood, hit.point, Quaternion.identity);
            }
            else
            {
                
                Instantiate(hitEffect, hit.point, Quaternion.identity);
            }

        }

        yield return new WaitForSeconds(shootRate);
        isShooting = false;
    }

    IEnumerator flashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();

        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            // Hey I'm Dead
            gameManager.instance.YouLose();
        }
    }

    public void Heal(int amount)
    {
        HP += amount;
        
        if (HP > HPOrig)
        {
            HP = HPOrig;
        }
        updatePlayerUI();

    }

    IEnumerator flashDamage()
    {
        gameManager.instance.playerFlashDamage.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        gameManager.instance.playerFlashDamage.SetActive(false);
    }

    void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerStaminaBar.fillAmount = (float)currentStamina / maxStamina;
    }

    public void spawnPlayer()
    {
        HP = HPOrig;
        updatePlayerUI();

        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public int GetHP()
    {
        return HP;
    }

    public int GetHPOrig()
    {
        return HPOrig;
    }

    public void pickUpGun(gunStats gun)
    {
        gunList.Add(gun);

        selectedGun = gunList.Count - 1;

        gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");
        gameManager.instance.ammoMaxText.text = gunList[selectedGun].ammoMax.ToString("F0");

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void gunSelect()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0 && selectedGun < gunList.Count - 1)
        {
            selectedGun++;
            changeGun();
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0 && selectedGun > 0)
        {
            selectedGun--;
            changeGun();
        }
    }
    public void changeGun()
    {

        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }



}




