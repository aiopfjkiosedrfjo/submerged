using Cinemachine;
using UnityEngine;
 
public class cameraRegister : MonoBehaviour
{
    private void OnEnable()
    {
        timelineManager.Register(GetComponent<CinemachineVirtualCamera>());
    }
    private void OnDisable()
    {
        timelineManager.Unregister(GetComponent<CinemachineVirtualCamera>());
    }
}