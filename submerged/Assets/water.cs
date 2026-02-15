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
    private AudioSource audioSource; // Audio source component
    public void Start()
    {
        Physics.gravity = originalGravity;
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = waterSound;
    }
    public void Update()
    {
        if (inWater)
        {
            print("buoyancy applied");
            timer1 += Time.deltaTime;
            if (!audioSource.isPlaying) audioSource.Play();
        }
    }
    public void OnTriggerEnter(Collider other)
    {
        if (!objectsInside.Contains(other.gameObject))
        {
            objectsInside.Add(other.gameObject);
            Physics.gravity = reducedGravity; // Reduce gravity by half
            inWater = true;

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
            audioSource.Stop();

        }
    }
}
