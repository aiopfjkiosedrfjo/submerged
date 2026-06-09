using UnityEngine;
using TMPro;

public class GPSController : MonoBehaviour
{
    public Transform GPSPOINT_1;
    public float distanceToPoint1;
    public TextMeshPro GPSTEXT;
    public Transform arrow;
    public void Update()
    {
        distanceToPoint1 = Vector3.Distance(transform.position, GPSPOINT_1.position);
        GPSTEXT.text = distanceToPoint1.ToString("F2") + "m";
        Vector3 directionToPoint1 = GPSPOINT_1.position - transform.position;
        float angleToPoint1 = Vector3.SignedAngle(transform.forward, directionToPoint1, Vector3.up);
        arrow.localRotation = Quaternion.Euler(0, 0, angleToPoint1);

    }
}
