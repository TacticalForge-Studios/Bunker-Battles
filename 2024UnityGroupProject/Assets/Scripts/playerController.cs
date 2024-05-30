using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour, IDamage, medkitHeal, experience
{
    [SerializeField] CharacterController controller;
    [SerializeField] GameObject gunModel;

    [SerializeField] int HP;
    [SerializeField] int speed;
    int origSpeed;
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
    [Range(0, 1)][SerializeField] float audJumpVol;

    [SerializeField] List<gunStats> gunList = new List<gunStats>();

    public ParticleSystem hitEffect;
    public ParticleSystem hitEffectBlood;


    Vector3 moveDir;
    Vector3 playerVel;

    int jumpCount;
    int HPOrig;
    int xp;
    float maxXP = 100;
    int currentLvl = 1;
    float xpModifier = 1.5f;
    float maxStamina = 100.0f;
    float currentStamina;
    float staminaDrain = 25.0f;
    float staminaRegen = 25.0f;
    int selectedGun;

    bool isShooting;
    bool isSprinting = false;

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = HP;
        origSpeed = speed;

        currentStamina = maxStamina;
        spawnPlayer();

    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
        gunSelect();
        movement();

    }

    void movement()
    {
        sprint();

        if (isSprinting)
        {
            currentStamina -= staminaDrain * Time.deltaTime;
            
            updatePlayerUI();



        }
        else if (!isSprinting && currentStamina < maxStamina)
        {
            currentStamina += staminaRegen * Time.deltaTime;
            if(currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
                updatePlayerUI();
            }
            updatePlayerUI();
        }
        if (currentStamina <= 0)
        {
            isSprinting = false;

        }



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
            isSprinting = true;
            speed *= sprintMod;


            if(currentStamina <= 0)
            {
                isSprinting = false;
            }

           
            
        }
        else if (Input.GetButtonUp("Sprint") || !isSprinting)
        {
            isSprinting = false;
            speed = origSpeed;

            if (currentStamina >= maxStamina)
            {
                currentStamina = maxStamina;
            }
            updatePlayerUI();
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

    public void giveXP(int amount)
    {
        xp += amount;
        updatePlayerUI();
        if(xp >= maxXP)
        {
            LevelUP((int)maxXP);
        }
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
        gameManager.instance.playerStaminaBar.fillAmount = currentStamina / maxStamina;
        gameManager.instance.playerXPBar.fillAmount = xp / maxXP;
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
        gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");
        gameManager.instance.ammoMaxText.text = gunList[selectedGun].ammoMax.ToString("F0");

        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
    }

    void setXPMax(float xp)
    {
        maxXP = xp;
    }

    public float LevelUP(int currentMaxXP)
    {
        float newXPMax = 0;
        currentLvl++;
        if (xp > maxXP)
        {
            int temp = xp - (int)maxXP;
            xp = temp;
        }
        newXPMax = currentMaxXP * xpModifier;
        setXPMax(newXPMax);
        gameManager.instance.LevelUp(currentLvl);
        
        return newXPMax;
    }

    public float getStaminaMax()
    {
        return maxStamina;
    }

    public void setHP(int health)
    {
        HPOrig = health;
    }

    public void setStamina(float stamina)
    {
        maxStamina = stamina;
    }


}




