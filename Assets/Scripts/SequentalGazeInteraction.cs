using UnityEngine;
using UnityEngine.Events;

public class SequentialGazeInteraction : MonoBehaviour
{
    public GameObject raySource;                  // The object emitting the ray (e.g., Main Camera)
    public int rayLength = 15;                    // Length of the ray
    public LayerMask layerMaskInteract;           // Assign appropriate layers to avoid false hits

    private bool isGazed = false;                 // Whether the object is currently being gazed at
    private int gazeCount = 0;                    // Track the number of gazes
    private bool actionTriggered = false;         // Track if the current gaze action has been triggered

    public UnityEvent FirstGaze;                  // Event for the first gaze
    public UnityEvent SecondGaze;                 // Event for the second gaze
    public UnityEvent ThirdGaze;                  // Event for the third gaze
    public UnityEvent GazeExit;                   // Event for gaze exit actions

    // First delayed actions and their durations for the second and third gaze events
    public UnityEvent SecondGazeFirstDelayed;
    public float secondGazeFirstDelay = 1f;

    public UnityEvent ThirdGazeFirstDelayed;
    public float thirdGazeFirstDelay = 1f;

    // Second delayed actions and their durations for the second and third gaze events
    public UnityEvent SecondGazeSecondDelayed;
    public float secondGazeSecondDelay = 2f;

    public UnityEvent ThirdGazeSecondDelayed;
    public float thirdGazeSecondDelay = 2f;

    private void Update()
    {
        // Create a forward direction based on the ray source's transform
        Vector3 direction = raySource.transform.forward;
        RaycastHit hit;

        Debug.DrawRay(raySource.transform.position, direction * rayLength, Color.red);

        // Cast a ray from the ray source
        if (Physics.Raycast(raySource.transform.position, direction, out hit, rayLength, layerMaskInteract))
        {
            if (hit.collider.gameObject == gameObject)
            {
                if (!isGazed)
                {
                    Debug.Log("Gaze Enter: " + gameObject.name);
                    isGazed = true;
                    actionTriggered = false; // Reset the action trigger
                    TriggerGazeAction(); // Trigger the appropriate action based on gaze count
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

    private void TriggerGazeAction()
    {
        if (actionTriggered) return; // Prevent multiple triggers for the same gaze

        // Increment gaze count and trigger the corresponding action
        gazeCount = (gazeCount % 3) + 1; // Cycle through 1, 2, 3
        actionTriggered = true; // Ensure action is only triggered once per gaze entry

        switch (gazeCount)
        {
            case 1:
                FirstGaze.Invoke();
                Debug.Log("First gaze action triggered.");
                break;
            case 2:
                SecondGaze.Invoke();
                Debug.Log("Second gaze action triggered.");
                Invoke("TriggerSecondGazeFirstDelayed", secondGazeFirstDelay);
                Invoke("TriggerSecondGazeSecondDelayed", secondGazeSecondDelay);
                break;
            case 3:
                ThirdGaze.Invoke();
                Debug.Log("Third gaze action triggered.");
                Invoke("TriggerThirdGazeFirstDelayed", thirdGazeFirstDelay);
                Invoke("TriggerThirdGazeSecondDelayed", thirdGazeSecondDelay);
                break;
        }
    }

    // Methods to trigger the first and second delayed events for the second gaze
    private void TriggerSecondGazeFirstDelayed()
    {
        Debug.Log("Second gaze first delayed action triggered.");
        SecondGazeFirstDelayed.Invoke();
    }

    private void TriggerSecondGazeSecondDelayed()
    {
        Debug.Log("Second gaze second delayed action triggered.");
        SecondGazeSecondDelayed.Invoke();
    }

    // Methods to trigger the first and second delayed events for the third gaze
    private void TriggerThirdGazeFirstDelayed()
    {
        Debug.Log("Third gaze first delayed action triggered.");
        ThirdGazeFirstDelayed.Invoke();
    }

    private void TriggerThirdGazeSecondDelayed()
    {
        Debug.Log("Third gaze second delayed action triggered.");
        ThirdGazeSecondDelayed.Invoke();
    }

    private void ResetGaze()
    {
        if (isGazed)
        {
            Debug.Log("Gaze Exit: " + gameObject.name);
            isGazed = false;
            actionTriggered = false; // Reset the action trigger for the next gaze

            // Trigger gaze exit actions
            GazeExit.Invoke();
        }
    }
}
