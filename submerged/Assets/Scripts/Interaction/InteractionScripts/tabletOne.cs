using UnityEngine;

public class tabletOne : MonoBehaviour, IInteractable
{
    [SerializeField] private NotificationSO NotificationData;
    [SerializeField] private NotificationSO NotificationData2;
    private float elapsedTime = 0f;
    public bool CanInteract()
    {
        return true;
    }
    public bool Interact(playerInteract playerInteract)
    {
        NotificationManager.Instance.ShowNotification(NotificationData);
        elapsedTime += Time.deltaTime;
        while (elapsedTime < 5f)
        {
            elapsedTime += Time.deltaTime;
            if (elapsedTime >= 5f)
            {
                gameManager.instance.UpdateCash(50000);
                NotificationManager.Instance.ShowNotification(NotificationData2);
            }
        }
        return true;
    }
}
