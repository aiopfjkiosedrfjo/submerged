using UnityEngine;

public class stationEvent : MonoBehaviour
{
    [SerializeField] private GameObject crematonge;
    [SerializeField] private Transform spawnPoint;
    [SerializeField] private AudioSource tabletRing;
    private bool hasSpanwed = false;
    private void OnTriggerEnter()
    {
        if (!hasSpanwed)
        {
            Instantiate(crematonge, spawnPoint.position, spawnPoint.rotation);
            hasSpanwed = true;
        }
    }
    private void Update()
    {
        if (gameManager.instance.NumberOfDives >= 3 && hasSpanwed == false)
        {
            tabletRing.volume =1f;
        }
    }
}
