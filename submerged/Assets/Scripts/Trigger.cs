using UnityEngine;

public class Trigger : MonoBehaviour
{
    [SerializeField] private triggerEnum trigger;
    [SerializeField] private triggerExitEnum triggerExit;
    [SerializeField] private bool triggered = false;

    private void OnTriggerEnter(Collider other)
    {
        if (!other.CompareTag("Player") || triggered)
        {
            return;
        }
        if (trigger != triggerEnum.None)
        {
            triggerManager.instance.Trigger(trigger);  
            triggered = true;
        } 
    }
    private void OnTriggerExit(Collider other)
    {
        if (!other.CompareTag("Player") || triggered)
        {
            return;
        }
        if (triggerExit != triggerExitEnum.None)
        {
            triggerManager.instance.TriggerExit(triggerExit);
            triggered = true;
        }
    }
}
