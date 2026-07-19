using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private triggerEnum trigger;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player"))
        {
            return;
        }
        triggerManager.instance.Trigger(trigger);
    }
}
