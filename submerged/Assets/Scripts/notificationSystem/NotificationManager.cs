using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class NotificationManager : MonoBehaviour
{
    public static NotificationManager Instance;

    [Header("Notification Settings")]
    [SerializeField] private GameObject notificationPrefab;
    [SerializeField] private TextMeshProUGUI notificationText;

    private CanvasGroup notificationCanvasGroup;
    private Queue<NotificationSO> notificationQueue = new Queue<NotificationSO>();
    private bool isDisplayingNotification = false;

    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
        
        notificationCanvasGroup = notificationPrefab.GetComponent<CanvasGroup>();
        notificationCanvasGroup.alpha = 0f;

    }
    
    public void ShowNotification(NotificationSO notificationSo)
    {
        notificationQueue.Enqueue(notificationSo);
        if (!isDisplayingNotification)
        {
            StartCoroutine(DisplayNotification());
        }
    }
    
    private System.Collections.IEnumerator DisplayNotification()
    {
        isDisplayingNotification = true;
        while (notificationQueue.Count > 0)
        {
            NotificationSO data = notificationQueue.Dequeue();
            notificationText.text = data.NotificationText;
            notificationCanvasGroup.alpha = 1f;
            yield return new WaitForSeconds(data.NotificationDuration);
            notificationCanvasGroup.alpha = 0f;
        }
        isDisplayingNotification = false;
    }
}
