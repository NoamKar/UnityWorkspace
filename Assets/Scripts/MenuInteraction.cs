using System.Collections; 
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

public class MenuInteraction : MonoBehaviour
{
    private Camera vrCamera;
    public int rayLength = 15;
    public LayerMask layerMaskInteract;
    public float highlightTime = 1.5f;
    public float selectTime = 2f;

    private bool isGazed = false;
    private float gazeTimer = 0f;
    private bool isHighlighted = false;
    private bool hasTriggeredSelection = false;

    public UnityEvent GazeEnter;
    public UnityEvent GazeHighlight;
    public UnityEvent GazeExit;
    public UnityEvent OnGazeSelection;

    [Header("Toggle Button Settings")]
    public bool isToggleButton = false;
    public bool toggleHasThreeStates = true;
    private int toggleState = 0;

    public UnityEvent OnFirstPress;
    public UnityEvent OnSecondPress;
    public UnityEvent OnThirdPress;

    public Image buttonImage;
    public Color normalColor = Color.white;
    public Color highlightColor = Color.yellow;
    public Color selectedColor = Color.green;

    private void Start()
    {
        FindVRCamera();
        if (buttonImage == null)
            buttonImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        FindVRCamera();
    }

    private void FindVRCamera()
    {
        if (vrCamera == null)
        {
            vrCamera = Camera.main;
            if (vrCamera == null)
            {
                Debug.LogError("VR Camera not found! Make sure the camera has the 'MainCamera' tag.");
            }
        }
    }

    private void Update()
    {
        if (vrCamera == null) return;

        Vector3 direction = vrCamera.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(vrCamera.transform.position, direction * rayLength, Color.green);

        if (Physics.Raycast(vrCamera.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isGazed)
                {
                    isGazed = true;
                    gazeTimer = 0f;
                    isHighlighted = false;
                    hasTriggeredSelection = false;
                    GazeEnter.Invoke();
                    ChangeButtonColor(highlightColor);
                    Debug.Log("Gaze Enter: " + gameObject.name);
                }

                gazeTimer += Time.unscaledDeltaTime; //  Use Unscaled Time!

                if (!isHighlighted && gazeTimer >= highlightTime)
                {
                    isHighlighted = true;
                    GazeHighlight.Invoke();
                    Debug.Log("Button Highlighted: " + gameObject.name);
                }

                if (!hasTriggeredSelection && gazeTimer >= selectTime)
                {
                    hasTriggeredSelection = true;

                    if (isToggleButton)
                    {
                        StartCoroutine(ToggleWithDelay());
                    }
                    else
                    {
                        OnGazeSelection.Invoke();
                        ChangeButtonColor(selectedColor);
                        Debug.Log("Button Selected: " + gameObject.name);
                        ResetGaze();
                    }
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
            isHighlighted = false;
            hasTriggeredSelection = false;
            GazeExit.Invoke();
            ChangeButtonColor(normalColor);
            Debug.Log("Gaze Exit: " + gameObject.name);
        }
    }

    private void ChangeButtonColor(Color newColor)
    {
        if (buttonImage != null)
        {
            buttonImage.color = newColor;
        }
    }

    private IEnumerator ToggleWithDelay()
    {
        yield return new WaitForSecondsRealtime(selectTime); //  Works even when timeScale = 0

        Debug.Log($"Before Toggle Update: {gameObject.name} - Current State: {toggleState}");

        // Execute the action for the current toggle state BEFORE changing the state
        if (toggleState == 0)
        {
            Debug.Log("Executing First Press Action");
            OnFirstPress.Invoke();
        }
        else if (toggleState == 1)
        {
            Debug.Log("Executing Second Press Action");
            OnSecondPress.Invoke();
        }
        else if (toggleState == 2)
        {
            Debug.Log("Executing Third Press Action");
            OnThirdPress.Invoke();
        }

        // Now update the toggle state AFTER executing the action
        if (toggleHasThreeStates)
        {
            toggleState = (toggleState + 1) % 3; // Cycles: 0  1  2  0
        }
        else
        {
            toggleState = (toggleState == 0) ? 1 : 0; // Cycles: 0  1  0
        }

        Debug.Log($"After Toggle Update: {gameObject.name} - New State: {toggleState}");

        ChangeButtonColor(selectedColor);
        ResetGaze();
    }


}
