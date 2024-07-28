using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;



public class PlayerController : MonoBehaviour, IDamage, IKnockbackable, IElementalDamage
{
    public static PlayerController instance;
    [SerializeField] CharacterController charController;
    [SerializeField] Cameracontroller cameraController;
    [SerializeField] AudioSource gunAud;

    [SerializeField] public float HP;
    [SerializeField] public int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] float crouchHeight;
    [SerializeField] int slideSpeed;
    public int money;
    [SerializeField] LayerMask hitLayers;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform ShootPos;
    [SerializeField] public GameObject gunModel;
    [SerializeField] GameObject armModel;
    [SerializeField] GameObject SwordModel;
    [SerializeField] float shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    [SerializeField] List<SwordStats> swordList = new List<SwordStats>();

    [SerializeField] private float meleeRange;
    [SerializeField] private int meleeDamage;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform meleeAttackPoint;
    [SerializeField] private float attackRate;
    [SerializeField] AudioClip fireSword;
    [SerializeField] AudioClip electricSword;
    [SerializeField] AudioClip acidSword;

    [SerializeField] public GameObject loadingIcon;
    public GameObject[]playerCollecibles;
    [SerializeField] gunStats[] guns;
    [SerializeField] SwordStats[] swords;
    [SerializeField] public GameObject damageIndicatorPrefab;
    Transform muzzleFlashPoint;
    private float nextAttackTime;

    float origHeight;
    float origCameraHeight = 1;
    int origSpeed;

    bool isSprinting = false;
    bool isCrouching = false;
    bool isReloading = false;
    bool isAiming;
    bool isPlayingSteps;
    public bool hasSword = false;

    bool isShooting;
    int jumpCount;
    public float HPorig;
    public float shopHP;

    public int currentAmmo;
    public int magazineSize;
    public int stockAmmo;
    int selectedGun;
    public float spreadAngle;
    public int pelletsFired;
    int selectedSword;



    Vector3 moveDir;
    Vector3 playerVel;
    Vector3 pushBack;

    public PlayerControls playerControls;
    public DamageEffect dmgEffect;
    public SaveSystem saveSystem;
    private Camera mainCamera;
    private Camera weaponCamera;
    private bool isAimingJustStarted = false;
    private float aimingCooldown = 0.21f;


    void Awake()
    {
        playerControls = new PlayerControls();
        instance= this;
        
    }

    private void OnEnable()
    {
        playerControls.Player.Enable();
        playerControls.Player.Jump.performed += OnJump;
        playerControls.Player.Crouch.performed += OnCrouch;
        playerControls.Player.Shoot.performed += OnShoot;
        playerControls.Player.Reload.performed += OnReload;

    }

    private void OnDisable()
    {
        playerControls.Player.Disable();
        playerControls.Player.Jump.performed -= OnJump;
        playerControls.Player.Crouch.performed -= OnCrouch;
        playerControls.Player.Shoot.performed -= OnShoot;
        playerControls.Player.Reload.performed -= OnReload;

    }

    //private void Start()
    //private SaveSystem saveSystem;

    // Start is called before the first frame update
    void Start()
    {
        playerCollecibles = CollectibleManager.instance.collectibleArr;
        playerControls = new PlayerControls();
        money=gameManager.instance.points;
        HP = gameManager.instance.saveSystem.LoadHP();
        HPorig = 100;
        Debug.Log("Player HP: " + HP);
        updatePlayerUI();
        LoadGuns();
        gunList[selectedGun].ammoCurr = magazineSize;
        spreadAngle = 10;
        pelletsFired = 8;
        origHeight = charController.height;
        origSpeed = speed;
        currentAmmo = magazineSize;
        muzzleFlashPoint = gunModel.transform.Find("MuzzleFlashPoint");
        mainCamera = Camera.main;

    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
            movement();
            if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurr > 0 && isShooting == false && !isReloading)
            {
                StartCoroutine(shoot());
            }
            if (Time.time >= nextAttackTime)
            {
                if (Input.GetKeyDown(KeyCode.V)) // Replace with your preferred key
                {
                    StartCoroutine(MeleeAttack());
                    nextAttackTime = Time.time + attackRate;
                }
            }
            if (Input.GetButton("Reload") && isReloading == false && !gameManager.instance.isPaused)
            {
                StartCoroutine(reload());
               
            }
            if (Input.GetButtonDown("Aim") || Input.GetButtonUp("Aim"))
            {
                StartCoroutine(ADS());
                StartCoroutine(DisableADS());
            }  
            if (Input.GetButtonDown("Sprint") || Input.GetButtonUp("Sprint"))
            {
                sprint();
                DisableSprint();
            }
        }
        selectWeapon();
        crouch();

        if (Input.GetKeyDown(KeyCode.L))
        {
            gameManager.instance.saveSystem.SaveHP(HP);
            StartCoroutine(loadIcon());
            SaveGuns(); // Save guns
            gameManager.instance.saveSystem.SavePoints(gameManager.instance.points);
            Debug.Log("Game Saved");
        }
        if (Input.GetKeyDown(KeyCode.P))
        {
            gameManager.instance.saveSystem.delete();
            Debug.Log("Save Deleted");
        }
    }

    public IEnumerator loadIcon()
    {
        loadingIcon.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        loadingIcon.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        loadingIcon.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        loadingIcon.SetActive(false);
        yield return new WaitForSeconds(0.75f);
        loadingIcon.SetActive(true);
        yield return new WaitForSeconds(0.75f);
        loadingIcon.SetActive(false);


    }

    void OnJump(InputAction.CallbackContext context)
    {
        jump();
    }

    void OnCrouch(InputAction.CallbackContext context)
    {
        crouch();
    }

    void OnShoot(InputAction.CallbackContext context)
    {
        StartCoroutine(shoot());
    }

    void OnReload(InputAction.CallbackContext context)
    {
        StartCoroutine(reload());
    }
    void movement()
    {
        if (charController.isGrounded)
        {
            jumpCount = 0;
            playerVel = Vector3.zero;
        }


        moveDir = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        charController.Move(moveDir * speed * Time.deltaTime);
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            AudioManager.instance.jumpSound();
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y -= gravity * 3 * Time.deltaTime;
        charController.Move((playerVel) * Time.deltaTime);
        if (charController.isGrounded && moveDir.magnitude > 0.3f && !isPlayingSteps)
            StartCoroutine(walkCycle());

    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && charController.isGrounded)
        {
            if (Input.GetButtonDown("Sprint") && charController.isGrounded)
            {
                isSprinting = true;
                speed *= sprintMod;
            }
        }
    }

    void DisableSprint()
    {
        if (Input.GetButtonUp("Sprint"))
        {
            if (isSprinting && speed != origSpeed)
            {
                isSprinting = false;
                speed /= sprintMod;
            }
        }
    }

    void jump()
    {
        if (charController.isGrounded && jumpCount < jumpMax)
        {
            AudioManager.instance.jumpSound();
            jumpCount++;
            playerVel.y = jumpSpeed;
        }
    }
    void crouch()
    {
        if (Input.GetButtonDown("Crouch"))
        {
            if (isSprinting)
            {
                StartCoroutine(Slide());
            }
            else if (!isCrouching)
            {
                isCrouching = true;
                charController.height = crouchHeight;
                cameraController.AdjustHeight(1);
            }
            else if (isCrouching)
            {
                isCrouching = false;
                charController.height = origHeight;
                cameraController.AdjustHeight(origCameraHeight);
            }
        }
    }

    IEnumerator Slide()
    {
        int initialSpeed = speed;
        speed = slideSpeed;
        charController.height = crouchHeight;
        cameraController.AdjustHeight(0);

        yield return new WaitForSeconds(1);

        charController.height = origHeight;
        cameraController.AdjustHeight(origCameraHeight);
        speed = origSpeed;
    }
    IEnumerator shoot()
    {
        if (currentAmmo > 0)
        {
            currentAmmo--;
            gunList[selectedGun].ammoCurr--;
            isShooting = true;
            gunAud.PlayOneShot(gunList[selectedGun].shootSound, gunList[selectedGun].shootVol);

            StartCoroutine(flashMuzzle());

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position + Vector3.forward, Camera.main.transform.forward, out hit, shootDistance))
            {
                if (gunModel.CompareTag("Shotgun") || gunModel.CompareTag("Suppressor"))
                {
                    shootShotgun();
                }
                else
                {
                    Vector3 targetPoint = GetTargetPoint();
                    Vector3 shootDirection = (targetPoint - ShootPos.position).normalized;

                    GameObject bulletInstance = Instantiate(bullet, ShootPos.position, Quaternion.LookRotation(shootDirection));
                    Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
                    rb.velocity = shootDirection * shootDistance * 2;

                    // Set the damage value of the bullet
                    Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
                    bulletScript.SetDamage(shootDamage);
                    // Instantiate effects based on what was hit
                    if (hit.collider.CompareTag("Enemy"))
                        Instantiate(gunList[selectedGun].enemyHitEffect, hit.point, Quaternion.identity);
                    else
                        Instantiate(gunList[selectedGun].hitEffect, hit.point, Quaternion.identity);
                }
            }
            else
            {
                // If nothing is hit, still instantiate the bullet
                GameObject bulletInstance = Instantiate(bullet, ShootPos.position, ShootPos.rotation);
                Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
                rb.velocity = ShootPos.forward * shootDistance * 2;

                // Set the damage value of the bullet
                Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
                bulletScript.SetDamage(shootDamage);
            }
            if (isAiming)
            {
                // Play ADS recoil animation based on gun model tag
                switch (gunModel.tag)
                {
                    case "RevRifle":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_RevolvingRifle_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_RevolvingRifle_Idle");
                            break;
                        }
                    case "Sniper":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Sniper_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Sniper_Idle");
                            break;
                        }

                    case "Pistol":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Pistol_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Pistol_Idle");
                            break;
                        }

                    case "Bullpup":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Bullpup_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Bullpup_Idle");
                            break;
                        }

                    case "Carbine":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Carbine_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Carbine_Idle");
                            break;
                        }

                    case "CompactCharger":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Compact_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Compact_Idle");
                            break;
                        }

                    case "Compensator":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Compensator_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Compensator_Idle");
                            break;
                        }

                    case "DrumPDW":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_DrumPDW_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_DrumPDW_Idle");
                            break;
                        }

                    case "AR":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_AR_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_AR_Idle_Recoil");
                            break;
                        }

                    case "Shotgun":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Shotgun_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Shotgun_Idle");
                            break;
                        }

                    case "Handgun":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Handgun_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Handgun_Idle");
                            break;
                        }

                    case "MicroSMG":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_MicroSMG_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_MicroSMG_Idle");
                            break;
                        }

                    case "SnubRevolver":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_SnubRevolver_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_SnubRevolver_Idle");
                            break;
                        }

                    case "Suppressor":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_Suppressor_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_Suppressor_Idle");
                            break;
                        }

                    case "Willy":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_WillySlapper_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_WillySlapper_Idle");
                            break;
                        }

                    case "WristBreaker":
                        {
                            armModel.GetComponent<Animator>().Play("ADS_WristBreaker_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("ADS_WristBreaker_Idle");
                            break;
                        }

                    default:
                        {
                            break;
                        }
                }
            }
            else
            {
                // Play hip-fire recoil animation based on gun model tag
                switch (gunModel.tag)
                {
                    case "RevRifle":
                        {
                            armModel.GetComponent<Animator>().Play("RevolvingRifle_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("RevolvingRifle_Idle");
                            if (cameraController != null)
                            {
                                Camera.main.fieldOfView = 60;
                            }
                            break;
                        }
                    case "Sniper":
                        {
                            armModel.GetComponent<Animator>().Play("Sniper_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Sniper_Idle");
                            if (cameraController != null)
                            {
                                Camera.main.fieldOfView = 60;
                            }
                            break;
                        }

                    case "Pistol":
                        {
                            armModel.GetComponent<Animator>().Play("Pistol_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Pistol_Idle");
                            break;
                        }

                    case "Bullpup":
                        {
                            armModel.GetComponent<Animator>().Play("Bullpup_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Bullpup_Idle");
                            break;
                        }

                    case "Carbine":
                        {
                            armModel.GetComponent<Animator>().Play("Carbine_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Carbine_Idle");
                            break;
                        }

                    case "CompactCharger":
                        {
                            armModel.GetComponent<Animator>().Play("Compact_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Compact_Idle");
                            break;
                        }

                    case "Compensator":
                        {
                            armModel.GetComponent<Animator>().Play("Compensator_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Compensator_Idle");
                            break;
                        }

                    case "DrumPDW":
                        {
                            armModel.GetComponent<Animator>().Play("DrumPDW_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("DrumPDW_Idle");
                            break;
                        }

                    case "AR":
                        {
                            armModel.GetComponent<Animator>().Play("AR_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("AR_Idle");
                            break;
                        }

                    case "Shotgun":
                        {
                            armModel.GetComponent<Animator>().Play("Shotgun_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Shotgun_Idle");
                            break;
                        }

                    case "Handgun":
                        {
                            armModel.GetComponent<Animator>().Play("Handgun_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Handgun_Idle");
                            break;
                        }

                    case "MicroSMG":
                        {
                            armModel.GetComponent<Animator>().Play("MicroSMG_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("MicroSMG_Idle");
                            break;
                        }

                    case "SnubRevolver":
                        {
                            armModel.GetComponent<Animator>().Play("SnubRevolver_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("SnubRevolver_Idle");
                            break;
                        }

                    case "Suppressor":
                        {
                            armModel.GetComponent<Animator>().Play("Suppressor_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("Suppressor_Idle");
                            break;
                        }

                    case "Willy":
                        {
                            armModel.GetComponent<Animator>().Play("WillySlapper_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("WillySlapper_Idle");
                            break;
                        }

                    case "WristBreaker":
                        {
                            armModel.GetComponent<Animator>().Play("WristBreaker_Recoil");
                            yield return new WaitForSeconds(shootRate);
                            armModel.GetComponent<Animator>().Play("WristBreaker_Idle");
                            break;
                        }
                   
                    default:
                        {
                            break;
                        }
                }
            }
            isShooting = false;
        }
        else
        {
            StartCoroutine(AudioManager.instance.gunEmpty(gunAud, shootRate));
            Debug.Log("Out of Ammo!");
        }
    }

    void shootShotgun()
    {
        if (gunModel.CompareTag("Shotgun") || gunModel.CompareTag("Suppressor"))
        {
            for (int i = 0; i < pelletsFired; ++i)
            {

                GameObject bulletInstance = Instantiate(bullet, ShootPos.position, ShootPos.rotation);
                Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();

                // Apply random spread to the bullet's direction using angles
                float angleX = UnityEngine.Random.Range(-spreadAngle, spreadAngle);
                float angleY = UnityEngine.Random.Range(-spreadAngle, spreadAngle);

                // Calculate the spread direction
                Vector3 spreadDirection = Quaternion.Euler(angleX, angleY, 0) * ShootPos.forward;

                bulletInstance.transform.position = ShootPos.position;
                bulletInstance.transform.rotation = Quaternion.LookRotation(spreadDirection);
                Ray ray = new Ray(ShootPos.position, spreadDirection);
                RaycastHit hit;
                if (Physics.Raycast(ray, out hit, shootDistance))
                {
                    // Set the bullet's transform to apply the spread
                    rb.velocity = spreadDirection * shootDistance;

                    Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
                    bulletScript.SetDamage(shootDamage);
                    if (hit.collider.CompareTag("Enemy"))
                        Instantiate(gunList[selectedGun].enemyHitEffect, hit.point, Quaternion.identity);
                }
            }
        }
    }
    IEnumerator flashMuzzle()
    {
        muzzleFlash.SetActive(true);
        yield return new WaitForSeconds(0.1f);
        muzzleFlash.SetActive(false);
    }
    IEnumerator ADS()
    {
        isAiming = true;
        isAimingJustStarted = true; // Set the flag indicating aiming has just started
        StartCoroutine(ResetAimingJustStarted()); // Start the coroutine to reset the flag
        if (Input.GetButtonDown("Aim"))
        {
            switch (gunModel.tag)
            {
                case "RevRifle":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_RevolvingRifle");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_RevolvingRifle_Idle");
                        if (cameraController != null)
                        {
                            Camera.main.fieldOfView = 20;
                        }
                        break;
                    } 
                case "Sniper":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Sniper");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Sniper_Idle");
                        if (cameraController != null)
                        {
                            Camera.main.fieldOfView = 20;
                        }
                        break;
                    }

                case "Pistol":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Pistol");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Pistol_Idle");
                        break;
                    }

                case "Bullpup":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Bullpup");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Bullpup_Idle");
                        break;
                    }

                case "Carbine":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Carbine");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Carbine_Idle");
                        break;
                    }

                case "CompactCharger":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Compact");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Compact_Idle");
                        break;
                    }

                case "Compensator":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Compensator");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Compensator_Idle");
                        break;
                    }

                case "DrumPDW":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_DrumPDW");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_DrumPDW_Idle");
                        break;
                    }
                case "AR":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_AR");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_AR_Idle");
                        break;
                    }
                case "Shotgun":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Shotgun");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Shotgun_Idle");
                        break;
                    }
                case "Handgun":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Handgun");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Handgun_Idle");
                        break;
                    }
                case "MicroSMG":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_MicroSMG");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_MicroSMG_Idle");
                        break;
                    }
                case "SnubRevolver":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_SnubRevolver");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_SnubRevolver_Idle");
                        break;
                    }
                case "Suppressor":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Suppressor");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_Suppressor_Idle");
                        break;
                    }         
                case "Willy":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_WillySlapper");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_WillySlapper_Idle");
                        break;
                    }
                case "WristBreaker":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_WristBreaker");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("ADS_WristBreaker_Idle");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }

    IEnumerator DisableADS()
    {
        if (Input.GetButtonUp("Aim"))
        {
            switch (gunModel.tag)
            {
                case "RevRifle":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_RevolvingRifle_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("RevolvingRifle_Idle");
                        if (cameraController != null)
                        {
                            Camera.main.fieldOfView = 60;
                        }
                        break;
                    }
                case "Sniper":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Sniper_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Sniper_Idle");
                        if (cameraController != null)
                        {
                            Camera.main.fieldOfView = 60;
                        }
                        break;
                    }

                case "Pistol":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Pistol_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Pistol_Idle");
                        break;
                    }

                case "Bullpup":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Bullpup_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Bullpup_Idle");
                        break;
                    }

                case "Carbine":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Carbine_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Carbine_Idle");
                        break;
                    }

                case "CompactCharger":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Compact_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Compact_Idle");
                        break;
                    }

                case "Compensator":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Compensator_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Compensator_Idle");
                        break;
                    }

                case "DrumPDW":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_DrumPDW_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("DrumPDW_Idle");
                        break;
                    }
                case "AR":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_AR_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("AR_Idle");
                        break;
                    }
                case "Shotgun":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Shotgun_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Shotgun_Idle");
                        break;
                    }
                case "Handgun":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Handgun_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Handgun_Idle");
                        break;
                    }
                case "MicroSMG":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_MicroSMG_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("MicroSMG_Idle");
                        break;
                    }
                case "SnubRevolver":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_SnubRevolver_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("SnubRevolver_Idle");
                        break;
                    }
                case "Suppressor":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_Suppressor_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("Suppressor_Idle");
                        break;
                    }
                case "Willy":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_WillySlapper_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("WillySlapper_Idle");
                        break;
                    }
                case "WristBreaker":
                    {
                        armModel.GetComponent<Animator>().Play("ADS_WristBreaker_Disable");
                        yield return new WaitForSeconds(0.2f);
                        armModel.GetComponent<Animator>().Play("WristBreaker_Idle");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
            isAiming = false;
        }
    }
    IEnumerator ReloadAnim()
    {
        if (isReloading)
        {
            switch (gunModel.tag)
            {
                case "RevRifle":
                    {
                        armModel.GetComponent<Animator>().Play("RevolvingRifle_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("RevolvingRifle_Idle");
                        break;
                    }
                case "Sniper":
                    {
                        armModel.GetComponent<Animator>().Play("Sniper_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Sniper_Idle");
                        break;
                    }

                case "Pistol":
                    {
                        Debug.Log("Animation Started");
                        armModel.GetComponent<Animator>().Play("Pistol_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Pistol_Idle");
                        Debug.Log("Animation Started");
                        break;
                    }

                case "Bullpup":
                    {
                        armModel.GetComponent<Animator>().Play("Bullpup_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Bullpup_Idle");
                        break;
                    }

                case "Carbine":
                    {
                        armModel.GetComponent<Animator>().Play("Carbine_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Carbine_Idle");
                        break;
                    }

                case "CompactCharger":
                    {
                        armModel.GetComponent<Animator>().Play("Compact_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Compact_Idle");
                        break;
                    }

                case "Compensator":
                    {
                        armModel.GetComponent<Animator>().Play("Compensator_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Compensator_Idle");
                        break;
                    }

                case "DrumPDW":
                    {
                        armModel.GetComponent<Animator>().Play("DrumPDW_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("DrumPDW_Idle");
                        break;
                    }
                case "AR":
                    {
                        armModel.GetComponent<Animator>().Play("AR_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("AR_Idle");
                        break;
                    }
                case "Shotgun":
                    {
                        armModel.GetComponent<Animator>().Play("Shotgun_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Shotgun_Idle");
                        break;
                    }
                case "Handgun":
                    {
                        armModel.GetComponent<Animator>().Play("Handgun_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Handgun_Idle");
                        break;
                    }
                case "MicroSMG":
                    {
                        armModel.GetComponent<Animator>().Play("MicroSMG_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("MicroSMG_Idle");
                        break;
                    }
                case "SnubRevolver":
                    {
                        armModel.GetComponent<Animator>().Play("SnubRevolver_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("SnubRevolver_Idle");
                        break;
                    }
                case "Suppressor":
                    {
                        armModel.GetComponent<Animator>().Play("Suppressor_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("Suppressor_Idle");
                        break;
                    }
                case "Willy":
                    {
                        armModel.GetComponent<Animator>().Play("WillySlapper_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("WillySlapper_Idle");
                        break;
                    }
                case "WristBreaker":
                    {
                        armModel.GetComponent<Animator>().Play("WristBreaker_Reload");
                        yield return new WaitForSeconds(2);
                        armModel.GetComponent<Animator>().Play("WristBreaker_Idle");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }
        }
    }

    public void takeDamage(float amount)
    {
        HP -= amount;
        StartCoroutine(DamageEffect.Instance.damageEffect());
        AudioManager.instance.hurtSound();
        updatePlayerUI();
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }
    public void takeFireDamage(float amount) { }
    public void takePoisonDamage(float amount) { }
    public void takeElectricDamage(float amount) { }
    public void takeExplosiveDamage(float amount)
    {
        HP -= amount;
    }

    void updatePlayerUI()
    {
        gameManager.instance.hpTarget = HP / HPorig;
        if (HP > 0)
        {
            gameManager.instance.drainHealthBar = StartCoroutine(gameManager.instance.DrainHealthBar());
        }
        else
        {
            gameManager.instance.playerHPBar.fillAmount = (float)HP / HPorig;
        }
        gameManager.instance.CheckHealthBar();
        shopHP = HP;
    }

    IEnumerator MeleeAttack()
    {
        if (hasSword && !isAimingJustStarted)
        {
            if (isAiming)
            {
               isAiming = false;
            }

            MeshRenderer gunMeshRenderer = gunModel.GetComponent<MeshRenderer>();

            if (gunMeshRenderer != null && hasSword)
            {
                gunMeshRenderer.enabled = false;
            }

            // Detect enemies in range
            Collider[] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, meleeRange, enemyLayer);

            // Apply damage to enemies
            foreach (Collider enemy in hitEnemies)
            {

                if (SwordModel.CompareTag("Electric"))
                {
                    IElementalDamage ElectricDamageable = enemy.GetComponent<IElementalDamage>();
                    if (ElectricDamageable != null)
                    {
                        ElectricDamageable.takeElectricDamage(meleeDamage);
                    }

                }
                else if (SwordModel.CompareTag("Acid"))
                {
                    IElementalDamage AcidDamageable = enemy.GetComponent<IElementalDamage>();
                    if (AcidDamageable != null)
                    {
                        AcidDamageable.takePoisonDamage(meleeDamage);
                    }

                }
                else if (SwordModel.CompareTag("Fire"))
                {
                    IElementalDamage FireDamageable = enemy.GetComponent<IElementalDamage>();
                    if (FireDamageable != null)
                    {
                        FireDamageable.takeFireDamage(meleeDamage);
                    }

                }
            }

            // Play sword swing animation
            armModel.GetComponent<Animator>().Play("Sword_Swing");

            yield return new WaitForSeconds(attackRate);

            // Re-enable gun model renderer
            if (gunMeshRenderer != null)
            {
                gunMeshRenderer.enabled = true;
            }

            switch (gunModel.tag)
            {
                case "RevRifle":
                    {
                        armModel.GetComponent<Animator>().Play("RevolvingRifle_Idle");
                        break;
                    }
                case "Sniper":
                    {

                        armModel.GetComponent<Animator>().Play("Sniper_Idle");
                        break;
                    }

                case "Pistol":
                    {

                        armModel.GetComponent<Animator>().Play("Pistol_Idle");
                        break;
                    }

                case "Bullpup":
                    {
                        armModel.GetComponent<Animator>().Play("Bullpup_Idle");
                        break;
                    }

                case "Carbine":
                    {
                        armModel.GetComponent<Animator>().Play("Carbine_Idle");
                        break;
                    }

                case "CompactCharger":
                    {
                        armModel.GetComponent<Animator>().Play("Compact_Idle");
                        break;
                    }

                case "Compensator":
                    {
                        armModel.GetComponent<Animator>().Play("Compensator_Idle");
                        break;
                    }

                case "DrumPDW":
                    {
                        armModel.GetComponent<Animator>().Play("DrumPDW_Idle");
                        break;
                    }
                case "AR":
                    {
                        armModel.GetComponent<Animator>().Play("AR_Idle");
                        break;
                    }
                case "Shotgun":
                    {
                        armModel.GetComponent<Animator>().Play("Shotgun_Idle");
                        break;
                    }
                case "Handgun":
                    {
                        armModel.GetComponent<Animator>().Play("Handgun_Idle");
                        break;
                    }
                case "MicroSMG":
                    {
                        armModel.GetComponent<Animator>().Play("MicroSMG_Idle");
                        break;
                    }
                case "SnubRevolver":
                    {
                        armModel.GetComponent<Animator>().Play("SnubRevolver_Idle");
                        break;
                    }
                case "Suppressor":
                    {
                        armModel.GetComponent<Animator>().Play("Suppressor_Idle");
                        break;
                    }
                case "Willy":
                    {
                        armModel.GetComponent<Animator>().Play("WillySlapper_Idle");
                        break;
                    }
                case "WristBreaker":
                    {
                        armModel.GetComponent<Animator>().Play("WristBreaker_Idle");
                        break;
                    }

                default:
                    {
                        break;
                    }
            }

        }
    }

    IEnumerator ResetAimingJustStarted()
    {
        yield return new WaitForSeconds(aimingCooldown);
        isAimingJustStarted = false;
    }

    void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(ShootPos.position, 1);
    }
    public IEnumerator reload()
    {
        isReloading = true;
        Debug.Log("Reloading");
        AudioManager.instance.reloadSound(gunAud);
        yield return StartCoroutine(ReloadAnim());
        int ammoToReload = Mathf.Min(magazineSize, stockAmmo);
        int neededAmmo = magazineSize - currentAmmo;

        if (stockAmmo >= neededAmmo)
        {
            currentAmmo = magazineSize;
            stockAmmo -= neededAmmo;
        }
        else
        {
            currentAmmo += stockAmmo;
            stockAmmo = 0;
        }

        gunList[selectedGun].ammoCurr = currentAmmo;
        isReloading = false;

    }

    public void IncreaseHealth()
    {
        if (HP + 20 > HPorig)
        {
            HP = HPorig;
        }
        else
        {
            HP += 20;
        }
        updatePlayerUI();
    }

    public void IncreaseSpeed()
    {
        speed += 1;
    }

    public void IncreaseStrength()
    {
        shootDamage += 5;
    }
    public void spinRoulette()
    {
        gunStats wonGun = guns[UnityEngine.Random.Range(0, guns.Length)];
        getGunStats(wonGun);
    }
    public void getGunStats(gunStats gun)
    {
        // If the player already has 2 guns, remove the currently equipped one
        if (gunList.Count >= 2)
        {
            gunStats removedGun = gunList[selectedGun];
            gunList.RemoveAt(selectedGun);

            // Adjust the selected gun index to avoid errors
            selectedGun = gunList.Count - 1;
        }

        gunList.Add(gun);
        selectedGun = gunList.Count - 1;

        shootDamage = gun.shootDmg;
        shootDistance = gun.shootDist;
        shootRate = gun.shootRate;
        currentAmmo = gun.ammoCurr;
        stockAmmo = gun.ammoMax;
        magazineSize = gun.magazineSize;
        gunModel.tag = gun.gunModel.tag;

        switch (gunModel.tag)
        {
            case "RevRifle":
                {
                    armModel.GetComponent<Animator>().Play("RevolvingRifle_Idle");
                    break;
                }
            case "Sniper":
                {

                    armModel.GetComponent<Animator>().Play("Sniper_Idle");
                    break;
                }

            case "Pistol":
                {

                    armModel.GetComponent<Animator>().Play("Pistol_Idle");
                    break;
                }

            case "Bullpup":
                {
                    armModel.GetComponent<Animator>().Play("Bullpup_Idle");
                    break;
                }

            case "Carbine":
                {
                    armModel.GetComponent<Animator>().Play("Carbine_Idle");
                    break;
                }

            case "CompactCharger":
                {
                    armModel.GetComponent<Animator>().Play("Compact_Idle");
                    break;
                }

            case "Compensator":
                {
                    armModel.GetComponent<Animator>().Play("Compensator_Idle");
                    break;
                }

            case "DrumPDW":
                {
                    armModel.GetComponent<Animator>().Play("DrumPDW_Idle");
                    break;
                }
            case "AR":
                {
                    armModel.GetComponent<Animator>().Play("AR_Idle");
                    break;
                }
            case "Shotgun":
                {
                    armModel.GetComponent<Animator>().Play("Shotgun_Idle");
                    break;
                }
            case "Handgun":
                {
                    armModel.GetComponent<Animator>().Play("Handgun_Idle");
                    break;
                }
            case "MicroSMG":
                {
                    armModel.GetComponent<Animator>().Play("MicroSMG_Idle");
                    break;
                }
            case "SnubRevolver":
                {
                    armModel.GetComponent<Animator>().Play("SnubRevolver_Idle");
                    break;
                }
            case "Suppressor":
                {
                    armModel.GetComponent<Animator>().Play("Suppressor_Idle");
                    break;
                }
            case "Willy":
                {
                    armModel.GetComponent<Animator>().Play("WillySlapper_Idle");
                    break;
                }
            case "WristBreaker":
                {
                    armModel.GetComponent<Animator>().Play("WristBreaker_Idle");
                    break;
                }

            default:
                {
                    break;
                }
        }

        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterials = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterials;

        // Adjust muzzle flash position and rotation based on the new weapon
        muzzleFlash.transform.localPosition = gun.muzzleFlashPositionOffset;
        muzzleFlash.transform.localRotation = Quaternion.Euler(gun.muzzleFlashRotationOffset);
    }
    public void getSwordStats(SwordStats sword)
    {
        // If the player already has 2 swords, remove the currently equipped one
        if (swordList.Count >= 2)
        {
            SwordStats removedSword = swordList[selectedSword];
            swordList.RemoveAt(selectedSword);

            // Adjust the selected sword index to avoid errors
            selectedSword = swordList.Count - 1;
        }

        swordList.Add(sword);
        selectedSword = swordList.Count - 1;



        meleeDamage = sword.swordDMG;

        SwordModel.tag = sword.SwordModel.tag;
        SwordModel.GetComponent<MeshFilter>().sharedMesh = sword.SwordModel.GetComponent<MeshFilter>().sharedMesh;
        SwordModel.GetComponent<MeshRenderer>().sharedMaterials = sword.SwordModel.GetComponent<MeshRenderer>().sharedMaterials;
    }

    void selectWeapon()
    {
        if (Input.GetAxis("Mouse ScrollWheel") > 0)
        {
            if (selectedGun < gunList.Count - 1)
            {
                selectedGun++;
                changeGun();
            }
            else if (selectedSword < swordList.Count - 1)
            {
                selectedSword++;
                changeSword();
            }
        }
        else if (Input.GetAxis("Mouse ScrollWheel") < 0)
        {
            if (selectedSword > 0)
            {
                selectedSword--;
                changeSword();
            }
            else if (selectedGun > 0)
            {
                selectedGun--;
                changeGun();
            }
        }
    }

    void changeGun()
    {
        AudioManager.instance.reloadSound(gunAud);
        shootDamage = gunList[selectedGun].shootDmg;
        shootDistance = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;
        currentAmmo = gunList[selectedGun].ammoCurr;
        stockAmmo = gunList[selectedGun].ammoMax;
        magazineSize = gunList[selectedGun].magazineSize;
        gunModel.tag = gunList[selectedGun].gunModel.tag;
        switch (gunModel.tag)
        {
            case "RevRifle":
                {
                    armModel.GetComponent<Animator>().Play("RevolvingRifle_Idle");
                    break;
                }
            case "Sniper":
                {

                    armModel.GetComponent<Animator>().Play("Sniper_Idle");
                    break;
                }

            case "Pistol":
                {

                    armModel.GetComponent<Animator>().Play("Pistol_Idle");
                    break;
                }

            case "Bullpup":
                {
                    armModel.GetComponent<Animator>().Play("Bullpup_Idle");
                    break;
                }

            case "Carbine":
                {
                    armModel.GetComponent<Animator>().Play("Carbine_Idle");
                    break;
                }

            case "CompactCharger":
                {
                    armModel.GetComponent<Animator>().Play("Compact_Idle");
                    break;
                }

            case "Compensator":
                {
                    armModel.GetComponent<Animator>().Play("Compensator_Idle");
                    break;
                }

            case "DrumPDW":
                {
                    armModel.GetComponent<Animator>().Play("DrumPDW_Idle");
                    break;
                }
            case "AR":
                {
                    armModel.GetComponent<Animator>().Play("AR_Idle");
                    break;
                }
            case "Shotgun":
                {
                    armModel.GetComponent<Animator>().Play("Shotgun_Idle");
                    break;
                }
            case "Handgun":
                {
                    armModel.GetComponent<Animator>().Play("Handgun_Idle");
                    break;
                }
            case "MicroSMG":
                {
                    armModel.GetComponent<Animator>().Play("MicroSMG_Idle");
                    break;
                }
            case "SnubRevolver":
                {
                    armModel.GetComponent<Animator>().Play("SnubRevolver_Idle");
                    break;
                }
            case "Suppressor":
                {
                    armModel.GetComponent<Animator>().Play("Suppressor_Idle");
                    break;
                }
            case "Willy":
                {
                    armModel.GetComponent<Animator>().Play("WillySlapper_Idle");
                    break;
                }
            case "WristBreaker":
                {
                    armModel.GetComponent<Animator>().Play("WristBreaker_Idle");
                    break;
                }

            default:
                {
                    break;
                }
        }

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterials = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterials;

        // Adjust muzzle flash position and rotation based on the new weapon
        muzzleFlash.transform.localPosition = gunList[selectedGun].muzzleFlashPositionOffset;
        muzzleFlash.transform.localRotation = Quaternion.Euler(gunList[selectedGun].muzzleFlashRotationOffset);
    }

    void changeSword()
    {
        meleeDamage = swordList[selectedSword].swordDMG;

        SwordModel.GetComponent<MeshFilter>().sharedMesh = swordList[selectedSword].SwordModel.GetComponent<MeshFilter>().sharedMesh;
        SwordModel.GetComponent<MeshRenderer>().sharedMaterials = swordList[selectedSword].SwordModel.GetComponent<MeshRenderer>().sharedMaterials;
    }
    IEnumerator walkCycle()
    {
        isPlayingSteps = true;
        AudioManager.instance.walkSound();
        if (!isSprinting)
        {
            yield return new WaitForSeconds(0.3f);
        }
        else
        {
            yield return new WaitForSeconds(0.25f);
        }
        isPlayingSteps = false;
    }


    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("MedKit"))
        {
            IncreaseHealth();
            // HP = Mathf.Clamp(HP + 30, 0, HPorig); // Adjust the amount of healing as needed
            updatePlayerUI();
            Destroy(other.gameObject);
        }

        if (other.CompareTag("SaveZone"))
        {
            gameManager.instance.saveSystem.SaveHP(HP);
            SaveGuns();
            gameManager.instance.saveSystem.SavePoints(gameManager.instance.points);
            //SaveSystem.instance.saveCollectibles();
            Debug.Log("Game Saved in SaveZone");
            StartCoroutine(loadIcon());
        }
    }
    public void SaveGuns()
    {
        for (int i = 0; i < guns.Length; i++)
        {
            PlayerPrefs.SetInt(guns[i].gunID, gunList.Contains(guns[i]) ? 1 : 0);
        }
        PlayerPrefs.Save();
    }

    public void LoadGuns()
    {

        for (int i = 0; i < guns.Length; i++)
        {
            if (PlayerPrefs.GetInt(guns[i].gunID, 0) == 1)
            {
                gunList.Add(guns[i]);
            }
        }
        changeGun();
    }

    //for the knockback to work with the player
    public void Knockback(Collider other, int lvl, int damage)
    {
        int force = 5;
        float knockbackDuration = 0.5f;
        float knockbackDistance = 3f;
        Vector3 knockBackDir = other.transform.forward;//gets direction of the enemy to apply to the player
        Vector3 targetPosition = transform.position+knockBackDir * knockbackDistance;

        StartCoroutine(ApplyKnockback(transform, targetPosition, knockbackDuration, force));
    }
    public IEnumerator ApplyKnockback(Transform playerTransform, Vector3 targetPosition, float duration, float force)
    {
        Vector3 initialPosition = playerTransform.position;
        float timer = 0f;

        while (timer < duration)
        {
            float progress = timer / duration;
            float currentSpeed = Mathf.Lerp(0, force, progress);

            playerTransform.position += (targetPosition + initialPosition).normalized * currentSpeed * Time.deltaTime;

            timer += Time.deltaTime;
            yield return null;
        }
    }
    public IEnumerator applyDamageOverTime(float amount, float duration, GameObject VFX) //the total damage over time in seconds
    {
        float timer = 0f;
        float damagePerSec = amount / duration;

        while (timer < duration)
        {
            float damagePerFrame = damagePerSec * Time.deltaTime;
            takeDamage(damagePerFrame);
            timer += Time.deltaTime;
            yield return null;
        }
        Destroy(VFX);
    }
    public void toggleSword()
    {
        //may lock later
        hasSword = true;

    }

    Vector3 GetTargetPoint()
    {
        if (isAiming)
        {
            // Assuming the crosshair is in the center of the screen
            Vector3 screenCenter = new Vector3(Screen.width / 2, Screen.height / 2, 0);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, hitLayers))
            {
                return hit.point;
            }
            else
            {
                return ray.GetPoint(shootDistance);
            }
        }
        else
        {
            // Assuming the crosshair is in the center of the screen
            Vector3 screenCenter = new Vector3(Screen.width / 2.1f, Screen.height / 1.9f, 0);
            Ray ray = mainCamera.ScreenPointToRay(screenCenter);

            if (Physics.Raycast(ray, out RaycastHit hit, shootDistance, hitLayers))
            {
                return hit.point;
            }
            else
            {
                return ray.GetPoint(shootDistance);
            }
        }
    }
}





