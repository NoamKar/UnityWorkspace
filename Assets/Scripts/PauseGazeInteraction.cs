using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class PauseGazeInteraction : MonoBehaviour
{
    private Camera vrCamera;            // Automatically detected camera
    public int rayLength = 15;          // Length of the ray
    public LayerMask layerMaskInteract; // Assign appropriate layers to avoid false hits
    public float gazeTriggerTime = 1f;  // Time in seconds required to trigger the action

    private bool isGazed = false;       // Whether the object is currently being gazed at
    private float gazeTimer = 0f;       // Timer to track gaze duration
    private bool hasTriggeredGazeEnter = false; // Ensure GazeEnter only happens once

    public UnityEvent GazeEnter;        // Event for additional actions on gaze enter (once)
    public UnityEvent GazeExit;         // Event for additional actions on gaze exit
    public UnityEvent OnGazeTrigger;    // Event when gaze trigger time is met

    [Header("Highlight Settings")]
    public Material objectMaterial;      // Public material reference
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;

    private MaterialPropertyBlock propertyBlock;
    private Renderer[] renderers; // Stores all Renderers in parent & children

    private void Start()
    {
        FindVRCamera();
        FindAllRenderers(); // Find all renderers in object and children
        propertyBlock = new MaterialPropertyBlock();
    }

    private void OnEnable()
    {
        FindVRCamera();
        FindAllRenderers();
        SceneManager.sceneLoaded += OnSceneLoaded; // Listen for scene changes
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded; // Unsubscribe on disable
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        FindVRCamera();
        FindAllRenderers();
    }

    private void FindVRCamera()
    {
        vrCamera = Camera.main;
        if (vrCamera == null)
        {
            Debug.LogError("VR Camera not found! Make sure the camera has the 'MainCamera' tag.");
        }
    }

    private void FindAllRenderers()
    {
        renderers = GetComponentsInChildren<Renderer>(); // Get all renderers in parent & children
    }

    private void Update()
    {
        if (vrCamera == null) return;

        Vector3 direction = vrCamera.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(vrCamera.transform.position, direction * rayLength, Color.red);

        if (Physics.Raycast(vrCamera.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isGazed)
                {
                    isGazed = true;
                    gazeTimer = 0f;
                    hasTriggeredGazeEnter = false;
                    GazeEnter.Invoke();
                    ChangeMaterialColor(highlightColor);
                    Debug.Log("Gaze Enter: " + gameObject.name);
                }

                gazeTimer += Time.deltaTime;

                if (gazeTimer >= gazeTriggerTime)
                {
                    OnGazeTrigger.Invoke();
                    Debug.Log("Gaze Duration Met. Action Triggered on: " + gameObject.name);
                    ResetGaze();
                }
            }
            else
            {
                ResetGaze();
            }
        }
        else
        {
            ResetGaze();
        }
    }

    private void ResetGaze()
    {
        if (isGazed)
        {
            isGazed = false;
            gazeTimer = 0f;
            GazeExit.Invoke();
            ChangeMaterialColor(normalColor);
            Debug.Log("Gaze Exit: " + gameObject.name);
        }
    }

    private void ChangeMaterialColor(Color newColor)
    {
        if (renderers == null) return;

        foreach (Renderer rend in renderers)
        {
            if (rend != null)
            {
                rend.GetPropertyBlock(propertyBlock);

                if (rend.sharedMaterial.HasProperty("_BaseColor")) // URP Lit Shader
                {
                    propertyBlock.SetColor("_BaseColor", newColor);
                }
                else if (rend.sharedMaterial.HasProperty("_Color")) // Standard Shader
                {
                    propertyBlock.SetColor("_Color", newColor);
                }

                rend.SetPropertyBlock(propertyBlock);
            }
        }
    }
}
