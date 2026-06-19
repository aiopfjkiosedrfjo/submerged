using UnityEngine;

public class outOfBounds : MonoBehaviour
{
    public water depthCheck;
    public float spawnRange;
    public GameObject fishPrefab;
    private bool hasSpawnedPredator;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (depthCheck.depth > 300)
        {
            SpawnPredator();
        }
        else
        {
            hasSpawnedPredator = false;
        }
    }
    public void SpawnPredator()
    {
        if (fishPrefab != null && !hasSpawnedPredator)
        {
            Vector3 spawnPos = transform.position + Random.insideUnitSphere * spawnRange;
            spawnPos.y = transform.position.y;
            Instantiate(fishPrefab, spawnPos, Quaternion.identity);
            hasSpawnedPredator = true;
        }
    }
}
