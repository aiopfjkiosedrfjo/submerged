using UnityEngine;

public class followTransform : MonoBehaviour
{
    [SerializeField] private Transform target;
    public Vector3 Offset = new Vector3(0f,0f,0f);
    void Update()
    {
        transform.rotation = target.rotation;
        if (Offset != Vector3.zero)
        {
            transform.position = target.position + Offset;
        }
        else
        {
            transform.position = target.position;
        }
    }
}
