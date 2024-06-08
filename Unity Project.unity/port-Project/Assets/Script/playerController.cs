using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour ,IDamage
{
    [SerializeField] CharacterController charController;
    [SerializeField] Cameracontroller cameraController;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    [SerializeField] float crouchHeight;
    [SerializeField] int slideSpeed;

    float origHeight;
    float origCameraHeight = 1;
    int origSpeed;
    bool isSprinting = false;
    bool isCrouching = false;
    bool isProne = false;
    bool isShooting;
    int jumpCount;
    int HPorig;

    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;
        origHeight = charController.height;
        origSpeed = speed;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        movement();
        sprint();
        crouch();
        if (Input.GetButton("Fire1") && isShooting == false)
            StartCoroutine(shoot());
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
            jumpCount++;
            playerVel.y = jumpSpeed;
        }

        playerVel.y -= gravity * Time.deltaTime;
        charController.Move(playerVel * Time.deltaTime);
    }

    void sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        { 
            isSprinting = true;
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {            
            isSprinting = false;
            speed /= sprintMod;
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
            else if (!isCrouching && !isProne)
            {
                isCrouching = true;
                isProne = false;
                cameraController.AdjustHeight(0.5f);
            }
            else if (isCrouching)
            {
                isCrouching = false;
                isProne = true;
                charController.height = crouchHeight;
                cameraController.AdjustHeight(0);
            }
            else if (isProne)
            {
                isProne = false;
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
        isShooting = true;
        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position + new Vector3(0, 0, 1), Camera.main.transform.forward, out hit, shootDistance))
        {
            Debug.Log(hit.transform.name);

            IDamage damage = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && damage != null)
            {
                damage.takeDamage(shootDamage);
            }
        }


        yield return new WaitForSeconds(shootRate);
        isShooting = false;
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
        gameManager.instance.playerHPBar.fillAmount = (float)HP / HPorig;
    }
}