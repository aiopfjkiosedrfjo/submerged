using UnityEngine;

public class stationEvent : MonoBehaviour
{
    [SerializeField] private GameObject crematonge;
    [SerializeField] private Transform spawnPoint;
    private bool hasSpanwed = false;
    private void OnTriggerEnter()
    {
        if (!hasSpanwed)
        {
            Instantiate(crematonge, spawnPoint.position, spawnPoint.rotation);
            hasSpanwed = true;
        }
    }
}
