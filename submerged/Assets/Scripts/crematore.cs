using UnityEngine;

public class crematore : MonoBehaviour
{
    [SerializeField] private float eventDuration = 40f;
    [SerializeField] private AudioClip screech;
    private float elapsedTime = 0f;
    private bool playedScreech = false;

    void Update()
    {
        transform.position += transform.forward * 10f * Time.deltaTime;
        elapsedTime += Time.deltaTime;

        if (elapsedTime >= eventDuration)
        {
            Destroy(gameObject);
        }
        else if (elapsedTime >= eventDuration - 32f)
        {
            if (playedScreech != true)
            {
                AudioSource.PlayClipAtPoint(screech, transform.position);
                playedScreech = true;
            }
        }
    }
}
