using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : MonoBehaviour
{

    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVerMax;
    [SerializeField] bool invertY;

    float rotX;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Update is called once per frame
    void Update()
    {

        //get input
        float mouseY = Input.GetAxis("Mouse Y") * Time.deltaTime * sens;
        float mouseX = Input.GetAxis("Mouse X") * Time.deltaTime * sens;

        if (invertY)
        {
            rotX += mouseY;
        }
        else
        {
            rotX -= mouseY;
        }

        //clamp the rotX on the X-Axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVerMax);

        //rotate the camera on the X-Axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //rotate the player on the Y-Axis
        transform.parent.Rotate(Vector3.up * mouseX);

    }
}
