using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cameracontroller : MonoBehaviour
{

    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;
    Vector3 originalLocalPosition;

    // Start is called before the first frame update
    void Start()
    {
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;
        originalLocalPosition = transform.localPosition;
    }

    // Update is called once per frame
    void Update()
    {

        //get input
        float mouseY = Input.GetAxis("Mouse Y") * sens * Time.deltaTime;
        float mouseX = Input.GetAxis("Mouse X") * sens * Time.deltaTime;

        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        // clamp the rotX on the X axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        //rotate the camera on the X axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        //rotate the player on the Y axis
        transform.parent.Rotate(Vector3.up * mouseX);

    }
    public void AdjustHeight(float height)
    {
        Vector3 newPosition = originalLocalPosition;
        newPosition.y = height;
        transform.localPosition = newPosition;
    }
}

