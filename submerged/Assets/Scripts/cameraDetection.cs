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
    [Header("Notification Data")]
    [SerializeField] private NotificationSO notificationData;
    [SerializeField] private NotificationSO cameraErrorNotCloseEnough;
    [Header("Camera Settings")]
    [SerializeField] private float importantDiscoveryRange = 20f;
    public float zoomLevel;
    public float maxZoom = 60;
    public float minZoom = 15;
    public float fishLimit = 10;
    public bool flashActive = false;
    public TextMeshPro flashText;
    public GameObject volumetricLight;
    public Camera photoCamera;
    public GameObject cameraFeed;
    public Material lastImage;
    public Material photoLiveFeedMaterial;
    public RenderTexture photoTexture;
    public RenderTexture LastImage;
    public Renderer[] photoTargets;
    public Image[] imageDisplay;
    public LayerMask targetLayer;
    public LayerMask importantDiscoveriesLayer;
    public Animator animator;
    public int count = 0;
    public TextMeshProUGUI photoCardInfo;
    public TextMeshProUGUI fishCountDisplay;
    public AudioClip cameraShutter;
    public AudioSource audioSource;
    public Material flashLight;
    public uiAnimations uiAnimations;
    private float distanceFromCamera;
    private string speciesNametemp;
    private float multiplier;
    private int combo;
    private bool isImportantDiscovery;
    private int totalMultiplier;
    private bool isCameraCloseUp = false;
    private Color originalFlashLightIntensity;
    public List<GameObject> ImportantDiscoveries = new List<GameObject>();
    private static readonly int photoHash = Animator.StringToHash("photo");
    private static readonly int zoomInHash = Animator.StringToHash("cameraCloseUp");
    private static readonly int zoomOutHash = Animator.StringToHash("cameraCloseUpReturn");
    public List<ImportantDiscoveriesData> ImportantDiscoveriesList = new List<ImportantDiscoveriesData>();
    [System.Serializable]
    public class ImportantDiscoveriesData
    {
        public Texture2D discoveryImage;
    }
    
    [System.Serializable]
    public class PhotoData
    {
        public Texture2D image;
        public float distanceFromCamera;
        public int multiplierIncrease;
        public float zoomLevel;
        public List<string> speciesName = new List<string>();
    }
    public List<PhotoData> photoDataList = new List<PhotoData>();
    public Image[] importantDiscoveriesPhotoUI;
    private int fishCount = 0;
    
    private List<Texture2D> capturedImages = new List<Texture2D>();
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        volumetricLight.SetActive(false);
        flashText.text = "OFF";
        photoCamera.targetTexture = photoTexture;
        originalFlashLightIntensity = flashLight.GetColor("_EmissionColor");
        
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && !isCameraCloseUp)
        {
            animator.SetTrigger(photoHash);
        }
        else if (Input.GetKeyDown(KeyCode.Mouse0) && isCameraCloseUp)
        {
            takePhoto();
        }
        if (Input.GetKeyDown(KeyCode.Mouse1) && !isCameraCloseUp)
        {
            animator.SetTrigger(zoomInHash);
            isCameraCloseUp = true;
            CameraCloseUp();
        }
        else if (Input.GetKeyDown(KeyCode.Mouse1) && isCameraCloseUp)
        {
            animator.SetTrigger(zoomOutHash);
            isCameraCloseUp = false;
            CameraCloseUpReturn();
        }
        if (Input.GetKeyDown(KeyCode.Q) && !flashActive)
        {
            flashActive = true;
            flashText.text = "ON";
        }
        else if (Input.GetKeyDown(KeyCode.Q) && flashActive)
        {
            flashActive = false;
            flashText.text = "OFF";
        }
        fishCountDisplay.text = $"{count}/{fishLimit} ";
        CameraSettings();
        
    }
    public System.Collections.IEnumerator FlashEffect()
    {
        flashLight.SetColor("_EmissionColor", Color.white * 20);
        volumetricLight.SetActive(true);
        yield return new WaitForSeconds(0.5f);
        flashLight.SetColor("_EmissionColor", originalFlashLightIntensity);
        volumetricLight.SetActive(false);
    }
    void CameraCloseUp()
    {
        cameraFeed.GetComponent<Renderer>().material = photoLiveFeedMaterial;
    }
    void CameraCloseUpReturn()
    {
        cameraFeed.GetComponent<Renderer>().material = lastImage;
    }
    void CameraSettings()
    {
        if (!isCameraCloseUp) return;
        float scroll = Input.mouseScrollDelta.y;
        if (scroll > 0)
        {
            photoCamera.fieldOfView = Mathf.Max(minZoom, photoCamera.fieldOfView - 5);
        }
        else if (scroll < 0)
        {
            photoCamera.fieldOfView = Mathf.Min(maxZoom, photoCamera.fieldOfView + 5);
        }
        

    }
    void takePhoto()
    {
        fishCount = 0;
        totalMultiplier=0;
        zoomLevel = photoCamera.fieldOfView;
        photoCamera.Render();
        Graphics.CopyTexture(photoTexture, LastImage);
        audioSource.PlayOneShot(cameraShutter);
        if (flashActive)
        {
            StartCoroutine(FlashEffect());
        }
        uiAnimations.CapturedImageAnimation();

        Plane[] planes = GeometryUtility.CalculateFrustumPlanes(photoCamera);
        foreach (GameObject game in globalFlock.allFish)
        {
            Renderer rend = game.GetComponentInChildren<SkinnedMeshRenderer>();
            if (isInView(planes, rend))
            {
                
                if (((1 << rend.gameObject.layer) & targetLayer) != 0)
                {
                    count++;
                    if (count > fishLimit) break;
                    game.GetComponent<Transform>();
                    distanceFromCamera = Vector3.Distance(photoCamera.transform.position, game.transform.position);
                    multiplier = gameManager.instance.AddMultiplier(distanceFromCamera);
                    speciesNametemp = LayerMask.LayerToName(rend.gameObject.layer);
                    Vector3 viewportPos = photoCamera.WorldToViewportPoint(game.transform.position);
                    multiplier *= CheckifVisible(viewportPos, rend, photoCamera)/10;
                    game.SetActive(false);
                    int multiplierINT = Mathf.RoundToInt(multiplier);
                    totalMultiplier += multiplierINT;
                    fishCount++;
                    
                }
            }
        }
        foreach (GameObject game in ImportantDiscoveries)
        {
            Renderer rend = game.GetComponent<MeshRenderer>(); 
            if (isInView(planes, rend))
            {
                if (((1 << rend.gameObject.layer) & importantDiscoveriesLayer) != 0)
                {
                    if (Vector3.Distance(photoCamera.transform.position, game.transform.position) < importantDiscoveryRange)
                    {
                        if (NotificationManager.Instance != null)
                        {
                            NotificationManager.Instance.ShowNotification(notificationData);
                        }
                        savePhoto(0, null, 1, true);
                    }
                    else
                    {
                        if (NotificationManager.Instance != null)
                        {
                            NotificationManager.Instance.ShowNotification(cameraErrorNotCloseEnough);
                        }
                    }
                }
            }
        }
        if (fishCount >0)
        {
            int extraPhotos = Mathf.Max(0, fishCount);
            savePhoto(totalMultiplier, speciesNametemp, extraPhotos, false);
        }
    }
    public float CheckifVisible(Vector3 viewportPos, Renderer rend, Camera cam)
    {
        Bounds bounds = rend.bounds;
        Vector3[] corners = {new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),
                    new Vector3(bounds.min.x, bounds.min.y, bounds.max.z),
                    new Vector3(bounds.min.x, bounds.max.y, bounds.min.z),
                    new Vector3(bounds.min.x, bounds.max.y, bounds.max.z),
                    new Vector3(bounds.min.x, bounds.min.y, bounds.min.z),

                    new Vector3(bounds.max.x, bounds.min.y, bounds.min.z),
                    new Vector3(bounds.max.x, bounds.min.y, bounds.max.z),
                    new Vector3(bounds.max.x, bounds.max.y, bounds.min.z),
                    new Vector3(bounds.max.x, bounds.max.y, bounds.max.z)};
        float minX = float.MaxValue;
        float maxX = float.MinValue;
        float minY = float.MaxValue;
        float maxY = float.MinValue;
        foreach (Vector3 corner in corners)
        {
            Vector3 vp = cam.WorldToViewportPoint(corner);

            minX = Mathf.Min(minX, vp.x);
            maxX = Mathf.Max(maxX, vp.x);

            minY = Mathf.Min(minY, vp.y);
            maxY = Mathf.Max(maxY, vp.y);
        }
        float clampedMinX = Mathf.Clamp01(minX);
        float clampedMaxX = Mathf.Clamp01(maxX);
        float clampedMinY = Mathf.Clamp01(minY);
        float clampedMaxY = Mathf.Clamp01(maxY);
        float width = Mathf.Max(0, clampedMaxX - clampedMinX);
        float height = Mathf.Max(0, clampedMaxY - clampedMinY);
        float screenCoverage = width * height;
        float sizeScore = Mathf.Clamp01(screenCoverage / 0.25f); 

        Vector2 center = new Vector2(0.5f, 0.5f);

        Vector2 fishCenter = new Vector2(
            (clampedMinX + clampedMaxX) * 0.5f,
            (clampedMinY + clampedMaxY) * 0.5f
        );

        float distance = Vector2.Distance(center, fishCenter);
        float centerScore = 1f - Mathf.Clamp01(distance / 0.5f);

        float score =
            (sizeScore * 0.6f) +
            (centerScore * 0.4f);

        return score * 100f;
    }
    void savePhoto(int multiplier, string speciesNametemp, int extraPhotos, bool isImportantDiscovery)
    {
        combo =0;
        Texture2D image = new Texture2D(LastImage.width, LastImage.height, TextureFormat.RGBAHalf, false);
        RenderTexture.active = LastImage;
        image.ReadPixels(new Rect(0, 0, LastImage.width, LastImage.height), 0, 0);
        image.Apply();
        if (!isImportantDiscovery)
        {
            PhotoData photodata = new PhotoData
            {
                image = image,
                distanceFromCamera = distanceFromCamera,
                multiplierIncrease = multiplier,
                zoomLevel = zoomLevel
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
        }
        else
        {
            ImportantDiscoveriesData discovery = new ImportantDiscoveriesData
            {
                discoveryImage = image
            };
            ImportantDiscoveriesList.Add(discovery);
            texture2dToSprite(image, true);
        }
        capturedImages.Add(image);
        texture2dToSprite(image, false);
    }
    void texture2dToSprite(Texture2D image, bool isImportantDiscovery)
    {
        if (!isImportantDiscovery)
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
        else
        {
            foreach (Image img in importantDiscoveriesPhotoUI)
            {
                if (img.sprite == null)
                {
                    img.sprite = Sprite.Create(image, new Rect(0, 0, image.width, image.height), new Vector2(0.5f, 0.5f));
                    return;
                }
            }
        }
    }

    bool isInView(Plane[] planes, Renderer renderer)
    {
        return GeometryUtility.TestPlanesAABB(planes, renderer.bounds);
    }

}
