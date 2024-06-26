using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class PlayerController : MonoBehaviour, IDamage
{

    [SerializeField] CharacterController charController;
    [SerializeField] Cameracontroller cameraController;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;
    [SerializeField] float crouchHeight;
    [SerializeField] int slideSpeed;

    [SerializeField] LayerMask hitLayers;
    [SerializeField] GameObject bullet;
    [SerializeField] Transform ShootPos;
    [SerializeField] GameObject gunModel;
    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;
    [SerializeField] GameObject muzzleFlash;
    [SerializeField] List<gunStats> gunList = new List<gunStats>();
    

    [SerializeField] private float meleeRange;
    [SerializeField] private int meleeDamage;
    [SerializeField] private LayerMask enemyLayer;
    [SerializeField] private Transform meleeAttackPoint;
    [SerializeField] private float attackRate;

    [SerializeField] gunStats[] guns;
    private float nextAttackTime;

    float origHeight;
    float origCameraHeight = 1;
    int origSpeed;

    bool isSprinting = false;
    bool isCrouching = false;
    bool isReloading = false;

    bool isShooting;
    int jumpCount;
    public int HPorig;
    public int shopHP;

    public int currentAmmo;
    public int magazineSize;
    public int stockAmmo;
    int selectedGun;
    public float spreadAngle;
    public int pelletsFired;


    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        spreadAngle = 10;
        pelletsFired = 8;
        HPorig = HP;
        origHeight = charController.height;
        origSpeed = speed;
        currentAmmo = magazineSize;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        if (!gameManager.instance.isPaused)
        {
            Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
            movement();
            if (isReloading)
                return;
            if (currentAmmo <= 0 && stockAmmo > 0)
            {
                StartCoroutine(reload());
                return;
            }
            if (Input.GetButton("Fire1") && gunList.Count > 0 && gunList[selectedGun].ammoCurr > 0 && isShooting == false)
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
        }
        selectGun();
        sprint();
        crouch();
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
        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax && charController.isGrounded)
        {
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y -= gravity * Time.deltaTime;
        charController.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint") && charController.isGrounded)
        {
            isSprinting = true;
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            if (isSprinting && speed != origSpeed) {
                isSprinting = false;
                speed /= sprintMod;
            }
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
            isShooting = true;
            StartCoroutine(flashMuzzle());

            RaycastHit hit;
            if (Physics.Raycast(Camera.main.transform.position + new Vector3(0, 0, 1), Camera.main.transform.forward, out hit, shootDistance))
            {
                if (gunList[selectedGun].gunModel.CompareTag("Shotgun"))
                {
                    shootShotgun();
                }
                else
                {
                    GameObject bulletInstance = Instantiate(bullet, ShootPos.position, ShootPos.rotation);
                    Rigidbody rb = bulletInstance.GetComponent<Rigidbody>();
                    rb.velocity = ShootPos.forward * shootDistance;

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
                rb.velocity = ShootPos.forward * shootDistance;

                // Set the damage value of the bullet
                Bullet bulletScript = bulletInstance.GetComponent<Bullet>();
                bulletScript.SetDamage(shootDamage);
            }

            yield return new WaitForSeconds(shootRate);
            isShooting = false;
        }
        else
        {
            Debug.Log("Out of Ammo!");
        }
    }

    void shootShotgun()
    {
        if (gunList[selectedGun].gunModel.CompareTag("Shotgun"))
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
                    if(hit.collider.CompareTag("Enemy"))
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

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();
        if (HP <= 0)
        {
            gameManager.instance.youLose();
        }
    }

    void updatePlayerUI()
    {
        gameManager.instance.hpTarget = (float)HP / HPorig;
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
        // Detect enemies in range
        Collider[] hitEnemies = Physics.OverlapSphere(meleeAttackPoint.position, meleeRange, enemyLayer);

        // Apply damage to enemies
        foreach (Collider enemy in hitEnemies)
        {
            IDamage damageable = enemy.GetComponent<IDamage>();
            if (damageable != null)
            {
                damageable.takeDamage(meleeDamage);
            }
        }

        yield return new WaitForSeconds(attackRate);
    }

    void OnDrawGizmosSelected()
    {
        if (meleeAttackPoint == null)
            return;

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(meleeAttackPoint.position, meleeRange);
    }
    public IEnumerator reload()
    {
        isReloading = true;
        Debug.Log("Reloading");
        yield return new WaitForSeconds(2);
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
        speed += 2;
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

        updatePlayerUI();

        shootDamage = gun.shootDmg;
        shootDistance = gun.shootDist;
        shootRate = gun.shootRate;
        currentAmmo = gun.ammoCurr;
        stockAmmo = gun.ammoMax;
        magazineSize = gun.magazineSize;

        gunModel.tag = gun.gunModel.tag;
        gunModel.GetComponent<MeshFilter>().sharedMesh = gun.gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterials = gun.gunModel.GetComponent<MeshRenderer>().sharedMaterials;
    }

    void selectGun()
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

    void changeGun()
    {
        updatePlayerUI();
        shootDamage = gunList[selectedGun].shootDmg;
        shootDistance = gunList[selectedGun].shootDist;
        shootRate = gunList[selectedGun].shootRate;
        currentAmmo = gunList[selectedGun].ammoCurr;
        stockAmmo = gunList[selectedGun].ammoMax;
        magazineSize = gunList[selectedGun].magazineSize;

        gunModel.GetComponent<MeshFilter>().sharedMesh = gunList[selectedGun].gunModel.GetComponent<MeshFilter>().sharedMesh;
        gunModel.GetComponent<MeshRenderer>().sharedMaterials = gunList[selectedGun].gunModel.GetComponent<MeshRenderer>().sharedMaterials;
    }
}

