using UnityEngine;

public class trashRoom : MonoBehaviour
{
    [SerializeField] private string playerTags; 
    [SerializeField] private Color ambientLightColor;
    [SerializeField] private water waterScript;
    [SerializeField] private float reflectionIntensity;
    private void OnTriggerEnter(Collider col)
    {
        if (col.CompareTag(playerTags))
        {
            lightingManager.instance.TrashRoomLighting(ambientLightColor, reflectionIntensity);
            waterScript.notInOtherAreas = false;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag(playerTags))
        {
            lightingManager.instance.RestoreLighting();
            waterScript.notInOtherAreas = true;
        }
    }
}
