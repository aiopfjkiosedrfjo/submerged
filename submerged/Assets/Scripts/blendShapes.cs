using UnityEngine;

public class blendShapes : MonoBehaviour
{
    SkinnedMeshRenderer skinnedMeshRenderer;
    Mesh skinnedMesh;

    int blendShapeCount;
    int playIndex = 0;

    float frameRate = 40f;
    float timer = 0f;

    void Start()
    {
        skinnedMeshRenderer = GetComponent<SkinnedMeshRenderer>();
        skinnedMesh = skinnedMeshRenderer.sharedMesh;
        blendShapeCount = skinnedMesh.blendShapeCount;
    }

    void Update()
    {
        timer += Time.deltaTime;

        if (timer >= 1f / frameRate)
        {
            timer -= 1f / frameRate;

            if (playIndex > 0)
                skinnedMeshRenderer.SetBlendShapeWeight(playIndex - 1, 0f);
            else
                skinnedMeshRenderer.SetBlendShapeWeight(blendShapeCount - 1, 0f);

            skinnedMeshRenderer.SetBlendShapeWeight(playIndex, 100f);

            playIndex++;

            if (playIndex >= blendShapeCount)
                playIndex = 0;
        }
    }
}