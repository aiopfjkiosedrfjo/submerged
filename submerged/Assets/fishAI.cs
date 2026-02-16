using System;
using UnityEngine;

public class fishAI : MonoBehaviour
{  
    public float turnSpeed = 1f;
    private float timer = 0f;
    public float speed = 1f;
    private float randomX = 0f;
    private float randomY = 0f;
    private float randomZ = 0f;

    void Update()
    {
        if (timer > 3f)
        {
            timer = 0f;
            GenerateRandomRotation();
        }
        else
        {
            timer += Time.deltaTime;
        }
        transform.rotation = Quaternion.Slerp(
            transform.rotation, 
            Quaternion.Euler(randomX, randomY, randomZ), 
            turnSpeed * Time.deltaTime
        );
        transform.position += transform.forward * speed * Time.deltaTime;
    }
    private void GenerateRandomRotation()
    {
        randomX = UnityEngine.Random.Range(-180f, 180f);
        randomY = UnityEngine.Random.Range(-90f, 90f);
        randomZ = UnityEngine.Random.Range(-180f, 180f);
    }
}
