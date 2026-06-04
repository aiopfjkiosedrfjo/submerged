using UnityEngine;

public class particleFollow : MonoBehaviour
{
    public Transform target;
    public ParticleSystem particle;

    // Update is called once per frame
    void LateUpdate()
    {
        if (target != null)
        {
            var main = particle.main;
            main.startSizeMultiplier = Mathf.Lerp(0.25f, 0.05f, target.position.y / 10f);
            transform.position = Vector3.Lerp(transform.position, target.position, Time.deltaTime / 1.2f);
        }
    }
}
