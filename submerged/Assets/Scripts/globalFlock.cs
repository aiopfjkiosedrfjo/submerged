using UnityEngine;
using System.Collections.Generic;

public class globalFlock : MonoBehaviour
{
    public GameObject fishPrefab;
    public GameObject delaidPrefab;
    public static int VERTICALtankSize = 7;
    public static int HORIZONTALtankSize = 50;
    public int VERTICALtankSizeLevi = 100;
    public int HORIZONTALtankSizeLevi = 200;
    public int numOfLeviathan = 2;
    public GameObject leviathanPrefab;
    static public int numFish = 500;
    public static GameObject[] allFish = new GameObject[numFish];
    public static Vector3 goalpos = Vector3.zero;
    public static Vector3 goalposLevi = Vector3.zero;
    public static Vector3 tankCenter;
    public List<GameObject> fishTraps = new List<GameObject>();

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        tankCenter = transform.position;
        createFish();
        createLeviathan();

    }

    // Update is called once per frame
    void Update()
    {
        if(Random.Range(0, 10000) < 50)
        {
            goalpos = tankCenter + new Vector3(Random.Range(-HORIZONTALtankSize, HORIZONTALtankSize)
                                        , Random.Range(-VERTICALtankSize, VERTICALtankSize)
                                        , Random.Range(-HORIZONTALtankSize, HORIZONTALtankSize));
        }
        if(Random.Range(0, 10000) < 50)
        {
            goalposLevi = tankCenter + new Vector3(Random.Range(-HORIZONTALtankSizeLevi, HORIZONTALtankSizeLevi)
                                        , Random.Range(-VERTICALtankSizeLevi, VERTICALtankSizeLevi)
                                        , Random.Range(-HORIZONTALtankSizeLevi, HORIZONTALtankSizeLevi));
        }
    }
    public void createFish()
    {
        for (int i = 0; i < numFish; i++)
        {
            Vector3 pos = tankCenter + new Vector3(Random.Range(-HORIZONTALtankSize, HORIZONTALtankSize)
                                                    , Random.Range(-VERTICALtankSize, VERTICALtankSize) 
                                                    , Random.Range(-HORIZONTALtankSize, HORIZONTALtankSize));
            GameObject prefabToSpawn = (i < numFish / 2) ? fishPrefab : delaidPrefab;
            allFish[i] = Instantiate(prefabToSpawn, pos, Quaternion.identity);
        }
        
    }
    public void createLeviathan()
    {
        for (int i = 0; i <numOfLeviathan; i++)
        {
            Vector3 pos = tankCenter + new Vector3(Random.Range(-HORIZONTALtankSizeLevi, HORIZONTALtankSizeLevi)
                                        , Random.Range(-VERTICALtankSizeLevi, VERTICALtankSizeLevi) 
                                        , Random.Range(-HORIZONTALtankSizeLevi, HORIZONTALtankSizeLevi));
            allFish[i] = Instantiate(leviathanPrefab, pos, Quaternion.identity);
        }
    }
}
