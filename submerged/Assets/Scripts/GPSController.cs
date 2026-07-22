using UnityEngine;
using TMPro;
public enum whatGPSPointIsActive
{
    GPSPOINT_01,
    GPSPOINT_02,
}
public class GPSController : MonoBehaviour
{
    public Transform GPSPOINT_1;
    public Transform GPSPOINT_2;
    public float distanceToPoint1;
    public Material pointMaterial;
    public TextMeshPro GPSTEXT;
    public Color pointColor = Color.red;
    public Color noPointColor = Color.gray;
    public float emmisionStrength = 1f;
    public whatGPSPointIsActive currentGPSpoint = whatGPSPointIsActive.GPSPOINT_01;
    public Transform arrow;
    [SerializeField] private NotificationSO gpsDestinationReached;
    [SerializeField] private bool notificationShown = false;
    public void Update()
    {
        switch (currentGPSpoint)
        {
            case whatGPSPointIsActive.GPSPOINT_01:
                CheckGPSLocation(GPSPOINT_1);
                break;

            case whatGPSPointIsActive.GPSPOINT_02:
                CheckGPSLocation(GPSPOINT_2);
                break;
        }

    }
    public void CheckGPSLocation(Transform gpsPoint)
    {
        distanceToPoint1 = Vector3.Distance(transform.position, gpsPoint.position);
        GPSTEXT.text = distanceToPoint1.ToString("F2") + "m";
        Vector3 directionToPoint1 = gpsPoint.position - transform.position;
        float angleToPoint1 = Vector3.SignedAngle(transform.forward, directionToPoint1, Vector3.up);
        arrow.localRotation = Quaternion.Euler(0, 0, angleToPoint1);
        float t = Mathf.Clamp01(distanceToPoint1 / 10f);
        Color currentColor = Color.Lerp(noPointColor, pointColor, t);
        pointMaterial.SetColor("_EmissionColor", currentColor * emmisionStrength);

        if (distanceToPoint1 < 20f && !notificationShown)
        {
            NotificationManager.Instance.ShowNotification(gpsDestinationReached);
            notificationShown = true;
        }
        else
        {
            notificationShown = false;
        }
    }
}
