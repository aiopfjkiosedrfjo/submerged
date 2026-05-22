using UnityEngine;

public class Camera_Controller : MonoBehaviour
{
    public Camera cam;
    public float targetFOV = 30f;
    public float zoomSpeed = 5f;
    public Animator zoomAnim;
    public float originalFov = 80f;

    public void Update()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            zoomAnim.SetBool("isZooming", true);
        }
        else
        {
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                originalFov,
                zoomSpeed * Time.deltaTime
            );
            zoomAnim.SetBool("isZooming", false);
        }
    }

    public void ZoomIn()
    {
        bool isZooming = zoomAnim.GetBool("isZooming");
        while (isZooming)
        {
            cam.fieldOfView = Mathf.Lerp(
                cam.fieldOfView,
                targetFOV,
                zoomSpeed * Time.deltaTime
            );
            isZooming = zoomAnim.GetBool("isZooming");
        }
    }
}
