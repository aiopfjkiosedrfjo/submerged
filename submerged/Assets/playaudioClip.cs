using UnityEngine;

public class playaudioClip : MonoBehaviour
{
    public AudioSource audioSource;
    public AudioClip screech;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.K))
        {
            audioSource.PlayOneShot(screech);
        }
    }
}
