using System;
using System.Collections.Generic;
using System.Timers;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Experimental.GlobalIllumination;
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
    public TextMeshProUGUI photoCardInfo;
    public AudioClip cameraShutter;
    public AudioSource audioSource;
    public Material flashLight;
    public uiAnimations uiAnimations;
    private float distanceFromCamera;
    private string speciesNametemp;
    private int multiplier;
    private int combo;
    private int totalMultiplier;
    private Color originalFlashLightIntensity;
    [System.Serializable]
    public class PhotoData
    {
        public Texture2D image;
        public float distanceFromCamera;
        public int multiplierIncrease;
        public List<string> speciesName = new List<string>();
    }
    public List<PhotoData> photoDataList = new List<PhotoData>();
    private int fishCount = 0;
    
    private List<Texture2D> capturedImages = new List<Texture2D>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        photoCamera.targetTexture = photoTexture;
        originalFlashLightIntensity = flashLight.GetColor("_EmissionColor");
        
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
        fishCount = 0;
        totalMultiplier=0;
        photoCamera.Render();
        Graphics.CopyTexture(photoTexture, LastImage);
        audioSource.PlayOneShot(cameraShutter);
        uiAnimations.CapturedImageAnimation();
        
        flashLight.SetColor("_EmissionColor",Color.white* 10);

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
                    multiplier = gameManager.instance.AddMultiplier(distanceFromCamera);
                    speciesNametemp = LayerMask.LayerToName(rend.gameObject.layer);
                    game.SetActive(false);
                    totalMultiplier += multiplier;
                    fishCount++;
                    
                }
            }
        }
        if (fishCount >0)
        {
            int extraPhotos = Mathf.Max(0, fishCount);
            savePhoto(totalMultiplier, speciesNametemp, extraPhotos);
        }
    }
    public void RestoreFlashLightIntensity()
    {
        flashLight.SetColor("_EmissionColor", originalFlashLightIntensity);
    }
    void savePhoto(int multiplier, string speciesNametemp, int extraPhotos)
    {
        combo =0;
        Texture2D image = new Texture2D(LastImage.width, LastImage.height, TextureFormat.RGBAHalf, false);
        RenderTexture.active = LastImage;
        image.ReadPixels(new Rect(0, 0, LastImage.width, LastImage.height), 0, 0);
        image.Apply();
        PhotoData photodata = new PhotoData
        {
            image = image,
            distanceFromCamera = distanceFromCamera,
            multiplierIncrease = multiplier,
        };
        if (extraPhotos > 0)
            for (int i = 0; i< extraPhotos; i++)
            {
                photodata.speciesName.Add(speciesNametemp);
                combo++;
            }
        else
        {
            photodata.speciesName.Add(speciesNametemp);
        }
        photoCardInfo.text = $"Species: {photodata.speciesName[0]} + {combo}, Multi {multiplier}";
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
