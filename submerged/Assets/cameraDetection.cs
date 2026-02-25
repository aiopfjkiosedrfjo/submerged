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
    public LayerMask targetLayer;
    public Animator animator;
    public int count = 0;
    private float distanceFromCamera;
    [System.Serializable]
    public class PhotoData
    {
        public Texture2D image;
        public float distanceFromCamera;
        public int multiplierIncrease;
    }
    public List<PhotoData> photoDataList = new List<PhotoData>();
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
        foreach (GameObject game in globalFlock.allFish)
        {
            Renderer rend = game.GetComponentInChildren<SkinnedMeshRenderer>();
            if (isInView(planes, rend))
            {
                
                if (((1 << rend.gameObject.layer) & targetLayer) != 0)
                {
                    count++;
                    if (count > 10) break;
                    game.GetComponent<Transform>();
                    distanceFromCamera = Vector3.Distance(photoCamera.transform.position, game.transform.position);
                    int multiplier = gameManager.instance.AddMultiplier(distanceFromCamera);
                    game.SetActive(false);
                    fishCount++;
                    savePhoto(multiplier);
                }
            }
        }
    }
    void savePhoto(int multiplier)
    {
        Texture2D image = new Texture2D(LastImage.width, LastImage.height, TextureFormat.RGBAHalf, false);
        RenderTexture.active = LastImage;
        image.ReadPixels(new Rect(0, 0, LastImage.width, LastImage.height), 0, 0);
        image.Apply();
        PhotoData photodata = new PhotoData
        {
            image = image,
            distanceFromCamera = distanceFromCamera,
            multiplierIncrease = multiplier
        };
        photoDataList.Add(photodata);
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
