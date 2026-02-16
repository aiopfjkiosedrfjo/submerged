using UnityEngine;

public class globalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public int tankSize = 5;
    static public int numFish = 20;
    public static GameObject[] allFish = new GameObject[numFish];
    public static Vector3 goalpos = Vector3.zero;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = transform.position + new Vector3(Random.Range(-tankSize, tankSize)
                                                    , Random.Range(-tankSize, tankSize)
                                                    , Random.Range(-tankSize, tankSize));

            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 10000) < 50)
        {
            goalpos = transform.position + new Vector3(Random.Range(-tankSize, tankSize)
                                        , Random.Range(-tankSize, tankSize)
                                        , Random.Range(-tankSize, tankSize));
        }
    }
}
