using UnityEngine;

public class lightingManager : MonoBehaviour
{
    public static lightingManager instance;
    public Color startingLightingColor;
    public float startingReflectionIntensity;
    private void Awake()
    {
        startingLightingColor = RenderSettings.ambientLight;
        startingReflectionIntensity = RenderSettings.reflectionIntensity;
        if (instance != null && instance != this)
        {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }
    public void TrashRoomLighting(Color ambientLightColor, float reflectionIntensity)
    {
        RenderSettings.ambientLight = Color.black;
        RenderSettings.reflectionIntensity = reflectionIntensity;
    }
    public void RestoreLighting()
    {
        RenderSettings.ambientLight = startingLightingColor;
        RenderSettings.reflectionIntensity = startingReflectionIntensity;
    }
}
