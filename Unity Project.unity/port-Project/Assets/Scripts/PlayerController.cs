using System.Collections;
using System.Collections.Generic;
using UnityEditor.Timeline.Actions;
using UnityEngine;

public class PlayerController : MonoBehaviour, IDamage
{
    [SerializeField] int HP;
    [SerializeField] float speed;
    [SerializeField] float sprintMod;
    [SerializeField] CharacterController controller;
    [SerializeField] int jump;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    //Guns
    [SerializeField] int shootDmg;
    [SerializeField] float shootRate;
    [SerializeField] int shootDist;

    bool isShooting;

    int HPOriginal;
    int jumpCount;
    Vector3 moveDirection;
    Vector3 playerVelocity;

    // Start is called before the first frame update
    void Start()
    {
        HPOriginal = HP;
        updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDist, Color.red);

        Movement();
        Sprint();

        if (Input.GetButton("Fire1") && !isShooting)
        StartCoroutine(shoot());
    }

    void Movement()
    {


        if (controller.isGrounded)
        {
            jumpCount = 0;
            playerVelocity = Vector3.zero;
        }

        moveDirection = Input.GetAxis("Horizontal") * transform.right + Input.GetAxis("Vertical") * transform.forward;
        controller.Move(moveDirection * speed * Time.deltaTime);

        if (Input.GetButtonDown("Jump") && jumpCount < jumpMax)
        {
            jumpCount++;
            playerVelocity.y = jumpSpeed;
        }

        playerVelocity.y -= gravity * Time.deltaTime;
        controller.Move(playerVelocity * Time.deltaTime);
    }

    void Sprint()
    {
        if (Input.GetButtonDown("Sprint"))
        {
            speed += sprintMod;
        }
        else if(Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
    }

    IEnumerator shoot()
    {
        isShooting = true;

        RaycastHit hit;
        if (Physics.Raycast(Camera.main.transform.position + new Vector3(1, 0, 0), Camera.main.transform.forward, out hit, shootDist))
        {
            Debug.Log(hit.transform.name);

            IDamage dmg = hit.collider.GetComponent<IDamage>();

            if (hit.transform != transform && dmg != null)
            {
                //Instantiate(cube, hit.point, Quaternion.identity);
                dmg.takeDamage(shootDmg);
            }

        }
            yield return new WaitForSeconds(shootRate);
        isShooting = false;

    }

    public void takeDamage(int amount)
    {
        HP -= amount;
        updatePlayerUI();

        if(HP <= 0)
        {
            GameManager.instance.youLose();
        }
    }

    void updatePlayerUI()
    {
        //GameManager.instance.playerHPBar.fillAmount = (float)HP / HPOriginal;

    }
}
