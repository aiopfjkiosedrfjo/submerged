using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using UnityEngine;


public class water : MonoBehaviour
{
    public float fogIntensity = 0.1f;
    public float buoyancyForce = 10f;
    public bool inWater = false;
    public Player playerController; // Reference to the player controller script
    private Color originalFogColor;
    private float timer1 = 0f;
    private Vector3 originalGravity = Physics.gravity;
    private Vector3 reducedGravity = new Vector3(0, -1, 0);
    public List<GameObject> objectsInside = new List<GameObject>();
    public AudioClip waterSound; 
    public AudioSource audioSourceOneShots; 
    public AudioSource audioSourceAmbience;
    public AudioClip waterAmbience1;
    public AudioClip waterSplash;
    public AudioClip waterExit;
    public Material fogShader;
    private bool waterSplashHasPlayed = false; 
    private bool waterExitHasPlayed = false; 
    public void Start()
    {
        fogShader.SetFloat("_Density", 0.03f);
        Physics.gravity = originalGravity;
        audioSourceAmbience.clip = waterSound;
        
    }
    public void Update()
    {
        if (inWater)
        {
            timer1 += Time.deltaTime;
            if (!audioSourceAmbience.isPlaying) audioSourceAmbience.Play();
            if (Random.Range(1f,500f) <= 1)
            {
                audioSourceAmbience.PlayOneShot(waterAmbience1);
            }
        }
        CheckSanityMeter();
    }
    public void CheckSanityMeter()
    {
        if (inWater)
        {
            gameManager.instance.sanityLevel -= Time.deltaTime / 5f;
            Debug.Log("Sanity Level: " + gameManager.instance.sanityLevel);
        }
    }

    public void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject) && other.gameObject.CompareTag("Player")) 
        {
            objectsInside.Add(other.gameObject);
            Physics.gravity = reducedGravity; 
            fogShader.SetFloat("_Density", fogIntensity);
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
            fogShader.SetFloat("_Density", 0.03f);
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
