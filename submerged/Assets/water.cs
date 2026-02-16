using System.Collections.Generic;
using System.Threading;
using JetBrains.Annotations;
using Unity.Mathematics;
using UnityEngine;

public class water : MonoBehaviour
{
    public float fogIntensity = 50f;
    public float buoyancyForce = 10f;
    public bool inWater = false;
    public Player playerController; // Reference to the player controller script
    private Color originalFogColor;
    public float originalIntensity = 400f;
    private float timer1 = 0f;
    private Vector3 originalGravity = Physics.gravity;
    private Vector3 reducedGravity = new Vector3(0, -1, 0);
    public List<GameObject> objectsInside = new List<GameObject>();
    public AudioClip waterSound; // Sound to play when entering water
    public AudioSource audioSourceOneShots; // Audio source component
    public AudioSource audioSourceAmbience;
    public AudioClip waterSplash;
    public AudioClip waterExit;
    private bool waterSplashHasPlayed = false; // Flag to track if the splash sound has been played
    private bool waterExitHasPlayed = false; // Flag to track if the exit sound has been played
    public void Start()
    {
        Physics.gravity = originalGravity;
        audioSourceAmbience.clip = waterSound;
        
    }
    public void Update()
    {
        if (inWater)
        {
            timer1 += Time.deltaTime;
            if (!audioSourceAmbience.isPlaying) audioSourceAmbience.Play();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject))
        {
            objectsInside.Add(other.gameObject);
            Physics.gravity = reducedGravity; // Reduce gravity by half
            inWater = true;
            if (!waterSplashHasPlayed)
            {
                Debug.Log("Playing splash sound");
                audioSourceOneShots.PlayOneShot(waterSplash); // Play splash sound when entering water
                waterSplashHasPlayed = true;
            }
            waterExitHasPlayed = false; // Reset exit sound flag when entering water

        }
    }
    public void OnTriggerExit(Collider other)
    {
        if (objectsInside.Contains(other.gameObject))
        {
            objectsInside.Remove(other.gameObject);
            inWater = false;
            Physics.gravity = originalGravity; // Reset gravity when not in water
            timer1 = 0f;
            if (!waterExitHasPlayed)
            {
                Debug.Log("Playing exit sound");
                audioSourceOneShots.PlayOneShot(waterExit); // Play exit sound when leaving water
                waterExitHasPlayed = true;
            }
            waterSplashHasPlayed = false; // Reset splash sound flag when exiting water
            audioSourceAmbience.Stop();
        }
    }
}
