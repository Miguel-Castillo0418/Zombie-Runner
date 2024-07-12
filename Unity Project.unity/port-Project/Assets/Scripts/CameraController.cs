using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Cameracontroller : MonoBehaviour
{

    [SerializeField] int sens;
    [SerializeField] int lockVertMin, lockVertMax;
    [SerializeField] bool invertY;

    float rotX;
    Vector3 originalLocalPosition;
    private PlayerControls playerControls;
    private Vector2 lookInput;

    // Start is called before the first frame update

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Player.Look.performed += OnLookPerformed;
        playerControls.Player.Look.Enable();
    }

    private void OnDisable()
    {
        playerControls.Player.Look.Disable();
    }

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
        float mouseY = (Input.GetAxis("Mouse Y") + lookInput.y) * sens * Time.deltaTime;
        float mouseX = (Input.GetAxis("Mouse X") + lookInput.x) * sens * Time.deltaTime;

        if (invertY)
            rotX += mouseY;
        else
            rotX -= mouseY;

        // Clamp the rotX on the X axis
        rotX = Mathf.Clamp(rotX, lockVertMin, lockVertMax);

        // Rotate the camera on the X axis
        transform.localRotation = Quaternion.Euler(rotX, 0, 0);

        // Rotate the player on the Y axis
        transform.parent.Rotate(Vector3.up * mouseX);

    }
    private void OnLookPerformed(InputAction.CallbackContext context)
    {
        lookInput = context.ReadValue<Vector2>();
    }
    public void AdjustHeight(float height)
    {
        Vector3 newPosition = originalLocalPosition;
        newPosition.y = height;
        transform.localPosition = newPosition;
    }
}