using System;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class triggerManager : MonoBehaviour
{
    public static triggerManager instance;
    [Header("SpotLightTrigger")]
    [SerializeField] private GameObject spotLight1;
    [SerializeField] private GameObject spotLight2;
    [SerializeField] private AudioSource audioSource;
    [SerializeField] private AudioClip spotLightSFX;
    private void Awake()
    {
        instance = this;
    }
    
    public void Trigger(triggerEnum trigger)
    {
        switch (trigger)
        {
            case triggerEnum.SpotLightTrigger1:
            SpotLightTrigger(spotLight1);
            break;
            case triggerEnum.SpotLightTrigger2:
            SpotLightTrigger(spotLight2);
            break;

        }
    }
    public void SpotLightTrigger(GameObject game)
    {
        game.SetActive(true);
        audioSource.PlayOneShot(spotLightSFX);
    }

}
