using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour //IDamage
{
    [SerializeField] CharacterController charController;

    [SerializeField] int HP;
    [SerializeField] int speed;
    [SerializeField] int sprintMod;
    [SerializeField] int jumpMax;
    [SerializeField] int jumpSpeed;
    [SerializeField] int gravity;

    [SerializeField] int shootDamage;
    [SerializeField] float shootRate;
    [SerializeField] int shootDistance;

    bool isShooting;
    int jumpCount;
    int HPorig;
    Vector3 moveDir;
    Vector3 playerVel;

    // Start is called before the first frame update
    void Start()
    {
        HPorig = HP;
        //updatePlayerUI();
    }

    // Update is called once per frame
    void Update()
    {
        Debug.DrawRay(Camera.main.transform.position, Camera.main.transform.forward * shootDistance, Color.red);
        movement();
        sprint();
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
            speed *= sprintMod;
        }
        else if (Input.GetButtonUp("Sprint"))
        {
            speed /= sprintMod;
        }
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

    //public void takeDamage(int amount)
    //{
    //    HP -= amount;
    //    updatePlayerUI();
    //    if (HP <= 0)
    //    {
    //        //Gamemanager.instance.youLose();
    //    }
    //}

    //void updatePlayerUI()
    //{
    //   // Gamemanager.instance.playerHPbar.fillAmount = (float)HP / HPorig;
    //}
}