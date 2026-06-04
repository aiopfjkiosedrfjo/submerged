using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using TMPro;
using UnityEngine;


public class water : MonoBehaviour
{
    public Player playerScript;
    public TextMeshProUGUI oxygenDisplay;
    public Transform player;
    public Light directionalLight;
    public float oxygenLevel = 60f;
    public float seaLevel = -13.85f;
    public float buoyancyForce = 10f;
    public bool inWater = false;
    public Color colorAtSurface;
    public Color colorAtDepth;
    public Player playerController; // Reference to the player controller script
    private float timer1 = 0f;
    private Vector3 originalGravity = Physics.gravity;
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
    private bool waterSplashHasPlayed = false; 
    private bool waterExitHasPlayed = false; 
    private float nextAmbienceTime;
    public float depth;
    public float maxDepth = 700f; // Define the maximum depth for clamping
    public void Start()
    {
        Physics.gravity = originalGravity;
        audioSourceAmbience.clip = waterSound;
        
    }
    public void Update()
    {
        if (inWater)
        {
            depth = Mathf.Abs(player.position.y - seaLevel);
            depthDisplay.text = "Depth: " + depth.ToString("F1") + "m";
            float t = Mathf.Clamp01(depth / maxDepth);
            VolumetricFog.color = Color.Lerp(colorAtSurface, colorAtDepth, t);
            directionalLight.intensity = Mathf.Lerp(3, 0.01f, t);
            VolumetricFog.SetFloat("_DensityMultiplier", Mathf.Lerp(0.05f, 0.15f, t));
            timer1 += Time.deltaTime;
            oxygenLevel = Mathf.Max(0, oxygenLevel - Time.deltaTime);
            oxygenDisplay.text = "Oxygen: " + oxygenLevel.ToString("F0");
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
            audioSourceAmbience.clip = aboveWaterAmbience;
            if (!audioSourceAmbience.isPlaying) audioSourceAmbience.Play();
            oxygenLevel = 60f;
            oxygenDisplay.text = "Oxygen: " + oxygenLevel.ToString("F0");
            VolumetricFog.SetFloat("_DensityMultiplier", 0.03f);
            directionalLight.intensity = 3;
            depthDisplay.text = "Depth: 0m";
        }
        if (oxygenLevel <= 0){
            playerScript.Die();
        }

        CheckSanityMeter();
    }
    public void CheckSanityMeter()
    {
        if (inWater)
        {
            gameManager.instance.sanityLevel -= Time.deltaTime / 5f;
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject) && other.gameObject.CompareTag("Player")) 
        {
            objectsInside.Add(other.gameObject);
            Physics.gravity = reducedGravity; 
            inWater = true;
            if (!waterSplashHasPlayed)
            {
                Debug.Log("Playing splash sound");
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
                Debug.Log("Playing exit sound");
                audioSourceOneShots.PlayOneShot(waterExit); 
                waterExitHasPlayed = true;
            }
            waterSplashHasPlayed = false; 
            audioSourceAmbience.Stop();
        }
    }
}
