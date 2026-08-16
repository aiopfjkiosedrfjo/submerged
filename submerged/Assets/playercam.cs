using UnityEngine;

public class playercam : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform orientation;
    public float originalValueMouseSensitivity;
    public float duration = 2f; 
    public bool hasReachedPosition = false;
    public float xRotation;
    public float yRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalValueMouseSensitivity = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.position = orientation.position;   
        

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }

}
