using UnityEngine;

public class globalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public static int tankSize = 20;
    static public int numFish = 20;
    public static GameObject[] allFish = new GameObject[numFish];
    public static Vector3 goalpos = Vector3.zero;
    public static Vector3 tankCenter;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tankCenter = transform.position;
        createFish();
    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 10000) < 50)
        {
            goalpos = tankCenter + new Vector3(Random.Range(-tankSize, tankSize)
                                        , Random.Range(-tankSize, tankSize)
                                        , Random.Range(-tankSize, tankSize));
        }
    }
    public void createFish()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = tankCenter + new Vector3(Random.Range(-tankSize, tankSize)
                                                    , Random.Range(-tankSize, tankSize)
                                                    , Random.Range(-tankSize, tankSize));

            allFish[i] = Instantiate(fishPrefab, pos, Quaternion.identity);
        }
        
    }
}
