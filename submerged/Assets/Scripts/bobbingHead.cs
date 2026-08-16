using System;
using Unity.Mathematics;
using UnityEngine;

[RequireComponent(typeof(followTransform))]
public class bobbingHead : MonoBehaviour
{
    [SerializeField] private float EffectIntensity;
    [SerializeField] private float EffectIntensityX;
    [SerializeField] private float EffectSpeed;

    private followTransform followerInstance;
    private Vector3 OriginalOffset;
    private float SinTime;

    private void Start()
    {
        followerInstance = GetComponent<followTransform>();
        OriginalOffset = followerInstance.Offset;
    }
    private void Update()
    {
        SinTime += Time.deltaTime * EffectSpeed;

        float bobY = Mathf.Sin(SinTime) * EffectIntensity;

        followerInstance.Offset = OriginalOffset + new Vector3(
            0f,
            bobY,
            0f
        );
    }
}
