
using UnityEngine;

public class uiAnimations : MonoBehaviour
{
    public GameObject capturedImagePrefab;
    public void CapturedImageAnimation()
    {

        LeanTween.scale(capturedImagePrefab, Vector3.one *1.2f, 0.3f)
                        .setEasePunch()
                        .setOnComplete(() => LeanTween.scale(capturedImagePrefab, Vector3.one, 0.15f));
    }
}
