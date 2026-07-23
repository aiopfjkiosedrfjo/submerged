using UnityEngine;

public class followTransform : MonoBehaviour
{
    [SerializeField] private Transform target;
    void Update()
    {
        transform.position = target.position;
        transform.rotation = target.rotation;
    }
}
