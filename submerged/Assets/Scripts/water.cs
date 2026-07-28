using System.Collections.Generic;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Rendering.Universal;
using Unity.VisualScripting;


public class water : MonoBehaviour
{
    public Player playerScript;
    public TextMeshProUGUI oxygenDisplay;
    public Transform player;
    public float oxygenLevel = 65f;
    public float MaxOxygen = 65f;
    public float seaLevel = -13.85f;
    public float buoyancyForce = 10f;
    public bool inWater = false;
    public Color colorAtSurface;
    public Color colorAtDepth;
    public Player playerController; // Reference to the player controller script
    private float timer1 = 0f;
    private Vector3 originalGravity = new Vector3(0, -9.81f, 0);
    private Vector3 reducedGravity = new Vector3(0, -1, 0);
    public List<GameObject> objectsInside = new List<GameObject>();
    public AudioClip waterSound; 
    public AudioSource audioSourceOneShots; 
    public AudioSource audioSourceAmbience;
    public AudioClip aboveWaterAmbience;
    public AudioClip waterAmbience1;
    public AudioClip waterSplash;
    public AudioClip waterExit;
    public TextMeshProUGUI depthDisplay;
    public Material VolumetricFog;
    public CanvasGroup canvasGroup;
    public bool fullyFaded = false;
    public float gracePeriodFadeOut = 4f;
    private bool waterSplashHasPlayed = false; 
    private bool waterExitHasPlayed = false; 
    public bool notInOtherAreas = true;
    private float nextAmbienceTime;
    public float depth;
    public bool HasTriggeredFadeOut = false;
    public float maxDepth = 700f;
    [SerializeField] private UniversalRendererData rendererData;
    [SerializeField] private string featureName = "THE NAME";
    [SerializeField] private string darkFogName = "DarkFogName";
    [SerializeField] private string waterDistortionName = "WaterDistortionName";
    [Header("Particle Systems")]
    [SerializeField] private ParticleSystem jumpInEffect;
    [Header("Settings")]
    [SerializeField] private bool darkFogToggle = false;
    public void Start()
    {
        Physics.gravity = originalGravity;
        audioSourceAmbience.clip = waterSound;
        
    }
    public void Update()
    {
        if (inWater)
        {
            ToggleFogShader(featureName, true);
            if(darkFogToggle) ToggleFogShader(darkFogName, false);
            ToggleFogShader(waterDistortionName, true);
            depth = Mathf.Abs(player.position.y - seaLevel);
            depthDisplay.text = "Depth: " + depth.ToString("F1") + "m";
            float t = Mathf.Clamp01(depth / maxDepth);

            //FOG
            VolumetricFog.color = Color.Lerp(colorAtSurface, colorAtDepth, t);
            VolumetricFog.SetFloat("_DensityMultiplier", Mathf.Lerp(0.01f, 0.09f, t));
            VolumetricFog.SetFloat("_MaxDistance", Mathf.Lerp(500f, 120f, t));

            //TIMER
            timer1 += Time.deltaTime;

            //OXYGEN
            oxygenLevel = Mathf.Max(0, oxygenLevel - Time.deltaTime);
            oxygenDisplay.text = "Oxygen: " + (oxygenLevel-5).ToString("F0");

            //SFX
            audioSourceAmbience.clip = waterSound;
            if (!audioSourceAmbience.isPlaying) audioSourceAmbience.Play();
            if (Time.time >= nextAmbienceTime)
            {
                Debug.Log("wefwsef");
                audioSourceOneShots.PlayOneShot(waterAmbience1);

                nextAmbienceTime = Time.time + Random.Range(10f, 40f);
            }
        }
        else
        {
            ToggleFogShader(featureName, false);
            if (darkFogToggle) ToggleFogShader(darkFogName, true);
            ToggleFogShader(waterDistortionName, false);
            //SFX
            audioSourceAmbience.clip = aboveWaterAmbience;
            if (!audioSourceAmbience.isPlaying) audioSourceAmbience.Play();
            if (!notInOtherAreas)
            {
                audioSourceAmbience.mute = true;
            }
            else
            {
                audioSourceAmbience.mute = false;
            }



            //OXYGEN
            oxygenLevel = MaxOxygen;
            oxygenDisplay.text = "Oxygen: " + oxygenLevel.ToString("F0");
            
            //FOG
            VolumetricFog.SetFloat("_MaxDistance", 120f);
            VolumetricFog.SetFloat("_DensityMultiplier", 0.03f);

            //Depth Text
            depthDisplay.text = "Depth: 0m";
        }
        if (oxygenLevel <= 0){
            playerScript.Die();
        }
        OxygenGracePeriod();

    }
    public void ToggleFogShader(string shaderName, bool whichToggle)
    {
        if (rendererData == null) return;
        ScriptableRendererFeature feature = rendererData.rendererFeatures.Find(f => f.name == shaderName);
        if (feature != null)
        {
            feature.SetActive(whichToggle);    
            rendererData.SetDirty();
        }

    }
    public void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject) && other.gameObject.CompareTag("Player")) 
        {
            objectsInside.Add(other.gameObject);
            Physics.gravity = reducedGravity; 
            inWater = true;
            jumpInEffect.Play();
            if (!waterSplashHasPlayed)
            {
                audioSourceOneShots.PlayOneShot(waterSplash); 
                waterSplashHasPlayed = true;
            }
            waterExitHasPlayed = false; 

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (objectsInside.Contains(other.gameObject))
        {
            objectsInside.Remove(other.gameObject);
            inWater = false;
            Physics.gravity = originalGravity; 
            timer1 = 0f;
            if (!waterExitHasPlayed)
            {
                audioSourceOneShots.PlayOneShot(waterExit); 
                waterExitHasPlayed = true;
            }
            waterSplashHasPlayed = false; 
            audioSourceAmbience.Stop();
        }
    }
    public void OxygenGracePeriod()
    {
        if(oxygenLevel <= 5 && !HasTriggeredFadeOut)
        {
            StartCoroutine(TriggerFadeOut(true, gracePeriodFadeOut));
            HasTriggeredFadeOut = true;
        }
        else
        {
            canvasGroup.alpha = 0f;
        }
    }
    public void ExternalScriptsTriggerFadeOut(bool isOxygen, float fadeOutTime)
    {
        StartCoroutine(TriggerFadeOut(isOxygen, fadeOutTime));
    }
    public IEnumerator TriggerFadeOut(bool isOxygen, float fadeOutTime)
    {
        float elapsed = 0f;

        while (elapsed < fadeOutTime)
        {
            elapsed += Time.deltaTime;

            float t = elapsed / fadeOutTime;
            canvasGroup.alpha = Mathf.Lerp(0f, 1.2f, t);

            yield return null;
        }

        canvasGroup.alpha = 1f; 
        if (isOxygen) HasTriggeredFadeOut = false;
    }

}
