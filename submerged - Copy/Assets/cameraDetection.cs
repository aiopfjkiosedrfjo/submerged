using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class cameraDetection : MonoBehaviour
{
    public Camera photoCamera;
    public RenderTexture photoTexture;
    public RenderTexture LastImage;
    public Renderer[] photoTargets;
    public Image[] imageDisplay;
    public Animator animator;
    private int fishCount = 0;
    
    private List<Texture2D> capturedImages = new List<Texture2D>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photoCamera.targetTexture = photoTexture;
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0))
        {
            animator.SetTrigger("photo");
        }
    }
    void takePhoto()
    {
        photoCamera.Render();
        Graphics.CopyTexture(photoTexture, LastImage);


        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(photoCamera);
        foreach (Renderer rend in photoTargets)
        {
            if (isInView(planes, rend))
            {
                rend.gameObject.SetActive(false);
                fishCount++;
                savePhoto();

            }
        }
    }
    void savePhoto()
    {
        Texture2D image = new Texture2D(LastImage.width, LastImage.height, TextureFormat.RGBA32, false);
        RenderTexture.active = LastImage;
        image.ReadPixels(new Rect(0, 0, LastImage.width, LastImage.height), 0, 0);
        image.Apply();
        capturedImages.Add(image);
        texture2dToSprite(image);
    }
    void texture2dToSprite(Texture2D image)
    {
        foreach (Image img in imageDisplay)
        {
            if (img.sprite == null)
            {
                img.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
                return;
            }
        }
        imageDisplay[0].sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
    }
    bool isInView(Plane[] planes, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

}
