using UnityEngine;

public class playercam : MonoBehaviour
{
    public float mouseSensitivity = 100f;
    public Transform orientation;
    public float originalValueMouseSensitivity;
    public bool isInteracting = false;
    public GameObject interactingObject;
    public Transform bookViewPos;
    public float duration = 2f; 
    private float elapsedTime;
    public bool hasReachedPosition = false;
    float xRotation;
    float yRotation;
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        originalValueMouseSensitivity = mouseSensitivity;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.J)){
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = !Cursor.visible;
            isInteracting = !isInteracting;
            elapsedTime = 0f;
            hasReachedPosition = false;
        } 
        float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * mouseSensitivity;
        float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * mouseSensitivity;

        yRotation += mouseX;
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);
        if (!isInteracting)
        {
            transform.position = orientation.position;   
        }
        if (isInteracting)
        {
            float percentage = LerpTimer();
            checkWhatIsInteracted(interactingObject, percentage);
            return;
        }

        transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
        orientation.rotation = Quaternion.Euler(0f, yRotation, 0f);
    }
    public void checkWhatIsInteracted(GameObject game, float percentageCompleted2)
    {
        Vector3 currentpos = orientation.position;
        Quaternion currentrot = orientation.rotation;
        Vector3 bookPos = bookViewPos.position;
        Quaternion bookRot = bookViewPos.rotation;
        //if ((game.CompareTag("book")))
        if (isInteracting)
        {
            transform.position = Vector3.Lerp(currentpos, bookPos, percentageCompleted2);
            transform.rotation = Quaternion.Lerp(currentrot, bookRot, percentageCompleted2);
        }
    }
    public float LerpTimer()
    {
        if (hasReachedPosition) return 1.0f;
        hasReachedPosition = false;
        elapsedTime += Time.deltaTime;
        float percentageCompleted = elapsedTime / duration;
        if (percentageCompleted >= 1.0f){
            elapsedTime = 0; 
            hasReachedPosition = true;
        } 
        return percentageCompleted;
    }
}
