using UnityEngine;

[CreateAssetMenu(fileName = "NotificationSO", menuName = "Scriptable Objects/NotificationSO")]
public class NotificationSO : ScriptableObject
{
    [SerializeField, TextArea] private string notificationText;
    [SerializeField] private float notificationDuration;
    [SerializeField] private float fadeInDuration;

    public string NotificationText => notificationText;
    public float NotificationDuration => notificationDuration;
    public float FadeInDuration => fadeInDuration;
}
