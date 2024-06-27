//using Palmmedia.ReportGenerator.Core.Reporting.Builders;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UIElements;

public class playerController : MonoBehaviour, IDamage, medkitHeal, experience, ammoResupply
{
    [SerializeField] CharacterController controller;
    [SerializeField] Animator anim;
    [SerializeField] GameObject gunModel;

    [SerializeField] int HP;
    [SerializeField] float Armor;
    [SerializeField] int speed;
    int origSpeed;
    int amountOfMedkits;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] int shootDist;
    [SerializeField] float shootRate;
    [SerializeField] GameObject muzzleFlashPistol;
    [SerializeField] GameObject muzzleFlashRifle;
    [SerializeField] GameObject muzzleFlashShotgun;
    [SerializeField] GameObject flashlight;
    [SerializeField] GameObject lowAmmo;
    [SerializeField] GameObject noAmmo;

    [SerializeField] TMP_Text healthLevel;
    [SerializeField] TMP_Text healthMax;

    [SerializeField] TMP_Text LevelNum;

    [SerializeField] TMP_Text StaminaCurrent;
    [SerializeField] TMP_Text StaminaMax;

    [SerializeField] TMP_Text xpCurrent;
    [SerializeField] TMP_Text xpMax;

    [SerializeField] TMP_Text medkitCount;

    [SerializeField] GameObject takingDamage;
    [SerializeField] GameObject takingArmDamage;
    [SerializeField] GameObject lowHealth;
    [SerializeField] GameObject almostDead;


    [SerializeField] AudioSource aud;
    [SerializeField] AudioClip[] audJump;
    [Range(0, 1)][SerializeField] float audJumpVol;
    [SerializeField] AudioClip audHeal;
    [Range(0, 1)][SerializeField] float audHealVol;
    [SerializeField] AudioClip audAmmoPickup;
    [Range(0, 1)][SerializeField] float audAmmoPickupVol;

    [SerializeField] AudioClip[] reloadSounds;
    [Range(0, 1)][SerializeField] float reloadSoundsVol;

    [SerializeField] AudioClip AmmoFailSound;
    [Range(0, 1)][SerializeField] float AmmoFailSoundVol;

    [SerializeField] AudioClip gunPickUpSound;
    [Range(0, 1)][SerializeField] float gunPickUpVol;

    [SerializeField] AudioClip footStepSounds;
    [Range(0, 1)][SerializeField] float footStepVol;

    //[SerializeField] AudioClip footstepSounds;
    //[Range(0, 1)][SerializeField] float footstepVol;

    [SerializeField] AudioClip deathSound;
    [Range(0, 1)][SerializeField] float deathVol;

    [SerializeField] public List<gunStats> gunList = new List<gunStats>();

    public ParticleSystem hitEffect;
    public ParticleSystem hitEffectBlood;


    Vector3 moveDir;
    Vector3 playerVel;
    Vector3 offsetPlayer;

    int jumpCount;
    int HPOrig;
    int ArmorOrig;
    int xp;
    float maxXP;
    int currenPlayerLvl;
    float xpModifier = 1.5f;
    float maxStamina;
    float currentStamina;
    float staminaDrain = 25.0f;
    float staminaRegen = 25.0f;
    float armorRecharge = 2.0f;
    float timeSinceTakenDamage;
    int selectedGun;
    int recoil;
    public float crouchSpeed = 2;
    public float normHeight = 2;
    public float crouchHeight = 0.2f;

    bool isShooting;
    bool isSprinting = false;
    bool armorBroken = false;
    bool flashlightOn = false;
    bool didGunChange = false;
    bool isCrouching = false;
    public static bool didPickUp;
    
    

    // Start is called before the first frame update
    void Start()
    {
        HPOrig = buttonFunctions.HP;
        ArmorOrig = (int)buttonFunctions.playerArmor;
        maxXP = buttonFunctions.maxXP;
        xp = buttonFunctions.xp;
        Armor = buttonFunctions.playerArmor;
        ArmorOrig = buttonFunctions.armorOrig;
        origSpeed = speed;
        currenPlayerLvl = buttonFunctions.currentPlayerLvl;
        maxStamina = buttonFunctions.stamina;
        currentStamina = maxStamina;
        gameManager.moneySaved = true;
        gameManager.instance.UpdateCurrencyText(buttonFunctions.currency);
        gameManager.moneySaved = false;
        spawnPlayer();
        if(MainMenu.isNewGame && buttonFunctions.gunsSaved)
        {
            buttonFunctions.gunList.Clear();
        }

        if (buttonFunctions.gunsSaved && !MainMenu.isNewGame)
        {
             setGunList(buttonFunctions.gunList);
            changeGun();
            
            
        }

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);
            gunSelect();
            movement();
        }
        timeSinceTakenDamage += Time.deltaTime;
        RechargeArmor();

        
    }
    void movement()
    {
        sprint();

        crouch();
    

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

        if (Input.GetButtonDown("Interact"))
        {

            if (Medkit.isInRange)
            {
                didPickUp = true;
                amountOfMedkits += 1;
                updatePlayerUI();
            }

            if (store.isInRange)
            {
                gameManager.instance.storeScript.OpenStore();
            }
        }

        if (Input.GetButtonDown("Heal"))
        {
            if(amountOfMedkits > 0)
            {
                if (HP < HPOrig)
                {
                    Heal(10);
                    amountOfMedkits -= 1;
                    if (HP > HPOrig)
                    {
                        HP = HPOrig;
                        updatePlayerUI();
                    }
                }
            }
            
        }


       
        if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurrent > 0 && !isShooting)
        {
            StartCoroutine(Shoot());

            
        }
        else if(Input.GetButtonDown("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurrent == 0 && !isShooting)
        {
           aud.PlayOneShot(AmmoFailSound, AmmoFailSoundVol);
        }



        if (Input.GetButtonDown("Reload"))
        {
            if (gunList[selectedGun].totalAmmoLeft > 0 && gunList[selectedGun].ammoCurrent != gunList[selectedGun].magCapacity)
            {
                if (gunList[selectedGun].gunModel.CompareTag("Pistol"))
                {
                    aud.PlayOneShot(reloadSounds[1], reloadSoundsVol);
                }
                else if(gunList[selectedGun].gunModel.CompareTag("Rifle"))
                {
                    aud.PlayOneShot(reloadSounds[2], reloadSoundsVol);
                }
                else if(gunList[selectedGun].gunModel.CompareTag("Shotgun"))
                {
                    aud.PlayOneShot(reloadSounds[0], reloadSoundsVol);
                }
                else 
                {
                    aud.PlayOneShot(reloadSounds[3], reloadSoundsVol);
                }

               

                if (gunList[selectedGun].ammoCurrent + gunList[selectedGun].totalAmmoLeft < gunList[selectedGun].magCapacity)
                {
                    int roundsShot = gunList[selectedGun].magCapacity - gunList[selectedGun].ammoCurrent;
                    gunList[selectedGun].ammoCurrent = gunList[selectedGun].ammoCurrent + gunList[selectedGun].totalAmmoLeft;
                    gunList[selectedGun].totalAmmoLeft = gunList[selectedGun].totalAmmoLeft - roundsShot;
                    
                }
                else
                {
                    int roundsShot = gunList[selectedGun].magCapacity - gunList[selectedGun].ammoCurrent;
                    gunList[selectedGun].ammoCurrent = gunList[selectedGun].magCapacity;
                    gunList[selectedGun].totalAmmoLeft = gunList[selectedGun].totalAmmoLeft - roundsShot;
                }

                
                if (gunList[selectedGun].totalAmmoLeft < 0)
                {
                    gunList[selectedGun].totalAmmoLeft = 0;
                }
                
                updatePlayerUI();
            }
            else
            {
                updatePlayerUI();
            }
            
        }

        if(Input.GetButtonDown("Toggle Flashlight"))
        {
            if (flashlightOn)
            {
                flashlight.SetActive(false);
                flashlightOn = false;
            }
            else
            {
                flashlight.SetActive(true);
                flashlightOn = true;
            }
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

    void crouch()
    {
        offsetPlayer = new Vector3(0, 1.5f, 0);

        if (Input.GetButtonDown("Crouch"))
        {
            isCrouching = !isCrouching;
        }

        if (isCrouching == true)
        {
            controller.height = controller.height - crouchSpeed * Time.deltaTime;
            speed /= 2;
            if (controller.height <= crouchHeight)
            {
                controller.height = crouchHeight;
            }
        }
        if (isCrouching == false)
        {
            controller.height = controller.height + crouchSpeed * Time.deltaTime;

            if (controller.height < normHeight)
            {
                transform.position = transform.position + offsetPlayer * Time.deltaTime;
            }
            if (controller.height >= normHeight)
            {
                controller.height = normHeight;
            }
        }
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


            if (hit.transform != transform && dmg != null && !hit.transform.CompareTag("MG"))
            {

                
                dmg.takeDamage(shootDamage);
                Instantiate(hitEffectBlood, hit.point, Quaternion.identity);

            } 
            else if (hit.transform.CompareTag("MG"))
            {
                machineGunTurret enemy = hit.transform.GetComponentInParent<machineGunTurret>();
                enemy.takeDamage(shootDamage * 2);
                Instantiate(hitEffect, hit.point, Quaternion.identity);
            }
            else if (hit.transform.CompareTag("Head"))
            {
                EnemyAI enemy = hit.transform.GetComponentInParent<EnemyAI>();
                enemy.takeDamage(shootDamage * 2);
                Instantiate(hitEffectBlood, hit.point, Quaternion.identity);

            }
            else if (hit.transform.CompareTag("MeleeHead"))
            {
                MeleeEnemy enemy = hit.transform.GetComponentInParent<MeleeEnemy>();
                enemy.takeDamage(shootDamage * 2);
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
        if (gunList[selectedGun].gunModel.CompareTag("Pistol"))
        {
            muzzleFlashPistol.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            muzzleFlashPistol.SetActive(false);
        }
        else if(gunList[selectedGun].gunModel.CompareTag("Rifle"))
        {
            muzzleFlashRifle.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            muzzleFlashRifle.SetActive(false);
        }
        else if (gunList[selectedGun].gunModel.CompareTag("Shotgun"))
        {
            muzzleFlashShotgun.SetActive(true);
            yield return new WaitForSeconds(0.1f);
            muzzleFlashShotgun.SetActive(false);
        }


    }

    void RechargeArmor()
    {
        
        

        if(Armor < ArmorOrig && timeSinceTakenDamage >= 5.0f)
        {
            Armor += armorRecharge * Time.deltaTime;
            armorBroken = false;
            if(Armor > ArmorOrig)
            {
                Armor = ArmorOrig;
            }
        }
    }

    public void takeDamage(int amount)
    {
        timeSinceTakenDamage = 0.0f;
        float currentArmor = Armor;
        Armor -= amount;
        if(Armor < 0)
        {
            Armor = 0;
        }

        if(Armor == 0)
        {
            if (armorBroken)
            {
                HP -= amount;
            }
            if (amount > currentArmor && !armorBroken)
            {
                float overDamage = (float)amount - currentArmor;
                
                HP -= (int)overDamage;
           
                armorBroken = true;
            }
            

        }
        

        updatePlayerUI();

        StartCoroutine(flashDamage());

        if (HP <= 0)
        {
            aud.PlayOneShot(deathSound, deathVol);
            // Hey I'm Dead
            gameManager.instance.YouLose();
        }
    }

    public void Heal(int amount)
    {
        HP += amount;
        aud.PlayOneShot(audHeal, audJumpVol);
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
        buttonFunctions.xp = xp;
    }

    IEnumerator flashDamage()
    {
        if (!armorBroken)
        {
            takingArmDamage.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            takingArmDamage.SetActive(false);
        }
        else
        {
            takingDamage.SetActive(true);
            yield return new WaitForSeconds(0.2f);
            takingDamage.SetActive(false);
        }
        
    }

    public void updatePlayerUI()
    {
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPOrig;
        gameManager.instance.playerStaminaBar.fillAmount = currentStamina / maxStamina;
        gameManager.instance.playerXPBar.fillAmount = xp / maxXP;
        gameManager.instance.playerArmorBar.fillAmount = Armor / ArmorOrig;

        healthLevel.text = HP.ToString("F0");
        healthMax.text = HPOrig.ToString("F0");
        
        LevelNum.text = currenPlayerLvl.ToString("F0");

        StaminaCurrent.text = currentStamina.ToString("F0");
        StaminaMax.text = maxStamina.ToString("F0");

        xpCurrent.text = xp.ToString("F0");
        xpMax.text = maxXP.ToString("F0");

        medkitCount.text = amountOfMedkits.ToString("F0");

        if (HP < HPOrig / 5)
        {
            lowHealth.SetActive(false);
            almostDead.SetActive(true);
        }
        else if (HP < HPOrig / 2)
        {
            lowHealth.SetActive(true);
            almostDead.SetActive(false);
        }
        else
        {
            lowHealth.SetActive(false);
            almostDead.SetActive(false);
        }

        if (gunList.Capacity != 0)
        {
            if (gunList[selectedGun].totalAmmoLeft <= gunList[selectedGun].ammoMax / 2)
            {
                noAmmo.SetActive(false);
                lowAmmo.SetActive(true);
            }
            else
            {
                noAmmo.SetActive(false);
                lowAmmo.SetActive(false);
            }
            if (gunList[selectedGun].totalAmmoLeft <= 0 && gunList[selectedGun].ammoCurrent == 0)
            {
                lowAmmo.SetActive(false);
                noAmmo.SetActive(true);
            }
            gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");
            gameManager.instance.ammoMaxText.text = gunList[selectedGun].totalAmmoLeft.ToString("F0");
        }
        

    }

    public void spawnPlayer()
    {
        HP = HPOrig;
        Armor = ArmorOrig;
        updatePlayerUI();

        controller.enabled = false;
        transform.position = gameManager.instance.playerSpawnPos.transform.position;
        controller.enabled = true;
    }

    public void Resupply(int amount)
    {
        gunList[selectedGun].totalAmmoLeft += amount;
        aud.PlayOneShot(audAmmoPickup, audAmmoPickupVol);

        if (gunList[selectedGun].totalAmmoLeft > gunList[selectedGun].ammoMax)
        {
            gunList[selectedGun].totalAmmoLeft = gunList[selectedGun].ammoMax;
        }
        updatePlayerUI();
    }

    public bool checkGunList()
    {
        if (gunList.Count == 0)
        {
            return false;
        }
        else
        {
            return true;
        }
    }

    public int getTotalAmmo()
    {
        return gunList[selectedGun].totalAmmoLeft;
    }

    public int getAmmoMax()
    {
        return gunList[selectedGun].ammoMax;
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
        aud.PlayOneShot(gunPickUpSound,gunPickUpVol);
        selectedGun = gunList.Count - 1;

        gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");
        gameManager.instance.ammoMaxText.text = gunList[selectedGun].totalAmmoLeft.ToString("F0");

        shootDamage = gun.shootDamage;
        shootDist = gun.shootDist;
        shootRate = gun.shootRate;
        recoil = gun.recoil;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        gunList[selectedGun].ammoCurrent = gunList[selectedGun].magCapacity;
        gunList[selectedGun].totalAmmoLeft = gunList[selectedGun].ammoMax;
        buttonFunctions.gunList.Add(gun);

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
        didGunChange = true;
        gameManager.instance.ammoCurrText.text = gunList[selectedGun].ammoCurrent.ToString("F0");
        gameManager.instance.ammoMaxText.text = gunList[selectedGun].totalAmmoLeft.ToString("F0");

        shootDamage = gunList[selectedGun].shootDamage;
        shootDist = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;
        recoil = gunList[selectedGun].recoil;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterial = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterial;
        didGunChange = false;
    }

    public void setXPMax(float xp)
    {
        maxXP = xp;
    }

    public float LevelUP(int currentMaxXP)
    {
        float newXPMax = 0;
        currenPlayerLvl++;
        if (xp > maxXP)
        {
            int temp = xp - (int)maxXP;
            xp = temp;
            buttonFunctions.xp = xp;
        }
        newXPMax = currentMaxXP * xpModifier;
        setXPMax(newXPMax);
        gameManager.instance.LevelUp(currenPlayerLvl);
        buttonFunctions.currentPlayerLvl = currenPlayerLvl;
        buttonFunctions.maxXP = newXPMax;
        return newXPMax;
    }

    public float getStaminaMax()
    {
        return maxStamina;
    }

    public void setHP(int health)
    {
        HPOrig = health;
        HP += 10;
    }

    public void setStamina(float stamina)
    {
        maxStamina = stamina;
    }

    public int getShootDamage()
    {
        return shootDamage;
    }

    public float getShootRate()
    {
        return shootRate;
    }

    public int getRecoil()
    {
        return recoil;
    }

    public int getLevel()
    {
        return currenPlayerLvl;
    }

    public void setLvl(int lvl)
    {
        currenPlayerLvl = lvl;
    }

    public void setXp(int Xp)
    {
        xp = Xp;
    }

    public int getXp()
    {
        return xp;
    }

    public void setArmor(int armorOrig)
    {
        ArmorOrig = armorOrig;
    }
    
    public bool getIsShooting()
    {
        return isShooting;
    }

    public bool getDidGunChange()
    {
        return didGunChange;
    }

    public void setCurrentHealth(int hp)
    {
        HP = hp;
    }

    public List<gunStats> getGunList()
    {
        return gunList;
    }

    public void setGunList(List<gunStats> GunList)
    {
        for (int i = 0; i < GunList.Count; i++)
        {
            gunList.Add(GunList[i]);
        }
        
        if(gunList.Count > 0)
        {
            Debug.Log("Guns added successfully");
        }
        else
        {
            Debug.Log("Guns failed to be added");
        }
    }
}




