using UnityEngine;

public class particleFollow : MonoBehaviour
{
    public Transform target;

    // Update is called once per frame
    void Update()
    {
        if (target != null)
        {
            transform.position = target.position;
        }
    }
}
